using InventoryService.API.Domain.Entities;
using InventoryService.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace InventoryServiceContainersTests;

public class InventoryServiceContainersTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithDatabase("inventoryDB")
        .WithUsername("postgres")
        .WithPassword("postgres.pass")
        .Build();

    private readonly RabbitMqContainer _rabbitMq = new RabbitMqBuilder().Build();

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        await _rabbitMq.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
        await _rabbitMq.DisposeAsync();
    }

    [Fact]
    public async Task Should_Connect_To_Postgres_And_Seed_Data()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgres.GetConnectionString()).Options;

        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Inventories.Add(new ProductInventory
        {
            ProductId = Guid.NewGuid(),
            AvailableStock = 10
        });
        await context.SaveChangesAsync();

        (await context.Inventories.CountAsync()).ShouldBe(1);
    }
}
