using Microsoft.EntityFrameworkCore;
using OrderService.API.Application.Interfaces;
using OrderService.API.Infrastructure.MassTransit;
using OrderService.API.Infrastructure.Persistence;
using OrderService.API.Infrastructure.Repositories;
var builder = WebApplication.CreateBuilder(args);
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
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();