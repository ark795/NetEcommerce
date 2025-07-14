using BasketService.API.Application.Consumers;
using BasketService.API.Application.Interfaces;
using BasketService.API.Application.Services;
using BasketService.API.Infrastructure.Data;
using MassTransit;
using Microsoft.AspNetCore.Components.Forms;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container. 
builder.Services.AddSingleton<RedisContext>();
builder.Services.AddScoped<IBasketService, BaskService>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductCreatedEventConsumer>();
    x.AddConsumer<ProductUpdatedEventConsumer>();
    x.AddConsumer<ProductDeletedEventConsumer>();
    x.UsingRabbitMq((ctx, cfg) =>
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

        cfg.ReceiveEndpoint("product-created-event-queue", e =>
        {
            e.ConfigureConsumer<ProductCreatedEventConsumer>(ctx);
        });
        cfg.ReceiveEndpoint("product-updated-event-queue", e =>
        {
            e.ConfigureConsumer<ProductUpdatedEventConsumer>(ctx);
        });
        cfg.ReceiveEndpoint("product-deleted-event-queue", e =>
        {
            e.ConfigureConsumer<ProductDeletedEventConsumer>(ctx);
        });
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