using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OrderService.API.Application.Interfaces;
using OrderService.API.Infrastructure.MassTransit;
using OrderService.API.Infrastructure.Persistence;
using OrderService.API.Infrastructure.Repositories;
using RabbitMQ.Client;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            //.AddEntityFrameworkCoreInstrumentation()
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "jaeger";
                options.AgentPort = 6831;
            })
            .AddSource("OrderService");

        tracerProviderBuilder.SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService("OrderService"));
    });

// ????? ???? Serilog
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Postgres")!)
    .AddRabbitMQ(sp =>
    {
        var factory = new RabbitMQ.Client.ConnectionFactory()
        {
            Uri = new Uri("amqp://guest:guest@rabbitmq:5672")
        };
        return factory.CreateConnectionAsync();
    }, name: "rabbitmq");

builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(15);
    options.MaximumHistoryEntriesPerEndpoint(60);
    options.AddHealthCheckEndpoint("OrderService", "/health");
})
.AddInMemoryStorage();

// Add services to the container. 
// PostgreSQL DbContext 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
builder.Services.AddMediatR(cfg =>
cfg.RegisterServicesFromAssemblies(typeof(OrderService.API.Application.Commands.CreateOrderCommand).Assembly));
// MassTransit + Outbox 
builder.Services.AddCustomMassTransit();
// Repositories 
builder.Services.AddScoped<IOrderService, OrderServiceImpl>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health");
    endpoints.MapHealthChecksUI(options =>
    {
        options.UIPath = "/health-ui";
    });
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();