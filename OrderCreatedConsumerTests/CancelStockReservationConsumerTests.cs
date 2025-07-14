using BuildingBlocks.Contracts.Dtos;
using BuildingBlocks.Contracts.Events;
using FluentAssertions;
using InventoryService.API.Domain.Entities;
using InventoryService.API.Infrastructure.Consumers;
using InventoryService.API.Infrastructure.Persistence;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace InventoryService.Tests.Consumers;

public class CancelStockReservationConsumerTests
{
    private readonly ITestHarness _harness;
    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceProvider _provider;

    public CancelStockReservationConsumerTests()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        services.AddScoped<CancelStockReservationConsumer>();
        services.AddLogging();

        services.AddMassTransitTestHarness(x =>
        {
            x.AddConsumer<CancelStockReservationConsumer>();

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        _provider = services.BuildServiceProvider();
        _harness = _provider.GetRequiredService<ITestHarness>();
        _dbContext = _provider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task Should_Consume_CancelStockReservation_And_Revert_Stock()
    {
        await _harness.Start();

        // Arrange
        var productId = Guid.NewGuid();

        _dbContext.Inventories.Add(new ProductInventory
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            AvailableStock = 3
        });
        await _dbContext.SaveChangesAsync();

        var bus = _provider.GetRequiredService<IBus>();

        // Act
        await bus.Publish(new CancelStockReservation
        {
            OrderId = Guid.NewGuid(),
            Items = new List<OrderItemMessage>
            {
                new()
                {
                    ProductId = productId,
                    Quantity = 2
                }
            }
        });

        // Assert
        await Task.Delay(200); // give time to consume

        var updatedInventory = await _dbContext.Inventories.FirstAsync(x => x.ProductId == productId);
        updatedInventory.AvailableStock.Should().Be(5);
    }
}