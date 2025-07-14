using InventoryService.API.Application.Interfaces;
using InventoryService.API.Infrastructure.Consumers;
using InventoryService.API.Infrastructure.Persistence;
using InventoryService.API.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container. 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));
builder.Services.AddScoped<IInventoryService, InventoryServiceImpl>();
builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<ApplicationDbContext>(o =>
    {
        o.UsePostgres();
        o.UseBusOutbox();
    });
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<OrderCreatedConsumer>();
    x.AddConsumer<CancelStockReservationConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        cfg.UseCircuitBreaker(cb =>
        {
            cb.TrackingPeriod = TimeSpan.FromMinutes(1);
            //cb.TripThreshold = 0.15;
            cb.TripThreshold = 1;
            cb.ActiveThreshold = 10;
            cb.ResetInterval = TimeSpan.FromMinutes(5);
        });

        cfg.ReceiveEndpoint("inventory-order-created", e =>
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
        cfg.ReceiveEndpoint("inventory-cancel-stock", e =>
        {
            e.ConfigureConsumer<CancelStockReservationConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});
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
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();