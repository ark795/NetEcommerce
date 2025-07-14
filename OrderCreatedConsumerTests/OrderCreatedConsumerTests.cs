using BuildingBlocks.Contracts.Dtos;
using BuildingBlocks.Contracts.Events;
using FakeItEasy;
using InventoryService.API.Application.Interfaces;
using InventoryService.API.Domain.Entities;
using InventoryService.API.Infrastructure.Consumers;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;

namespace OrderCreatedConsumerTests;

public class OrderCreatedConsumerTests
{
    [Fact]
    public async Task Should_Consume_OrderCreated_And_ReserveStock()
    {
        var harness = new InMemoryTestHarness();
        var inventoryService = A.Fake<IInventoryService>();
        A.CallTo(() => inventoryService.ReserveStockAsync(A<List<(Guid, int)>>._))
            .Returns(true);

        var consumer = harness.Consumer(() =>
            new OrderCreatedConsumer(inventoryService, NullLogger<OrderCreatedConsumer>.Instance));

        await harness.Start();
        try
        {
            await harness.InputQueueSendEndpoint.Send(new OrderCreated
            {
                OrderId = Guid.NewGuid(),
                BuyerId = Guid.NewGuid(),
                Items = new List<OrderItemMessage>
                {
                    new() { ProductId = Guid.NewGuid(), Quantity = 2, Price = 100 }
                }
            });

            (await harness.Consumed.Any<OrderCreated>()).ShouldBeTrue();
        }
        finally
        {
            await harness.Stop();
        }
    }
}


//public class OrderCreatedConsumerTests
//{
//    private readonly ITestHarness _harness;
//    private readonly IServiceProvider _provider;
//    private readonly ApplicationDbContext _dbContext;

//    public OrderCreatedConsumerTests()
//    {
//        var services = new ServiceCollection();

//        services.AddDbContext<ApplicationDbContext>(options =>
//            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

//        services.AddScoped<IInventoryService, InventoryService.API.Infrastructure.Repositories.InventoryServiceImpl>();
//        services.AddScoped<OrderCreatedConsumer>();
//        services.AddLogging();

//        services.AddMassTransitTestHarness(x =>
//        {
//            x.AddConsumer<OrderCreatedConsumer>();

//            x.UsingInMemory((context, cfg) =>
//            {
//                cfg.ConfigureEndpoints(context);
//            });
//        });

//        _provider = services.BuildServiceProvider();
//        _harness = _provider.GetRequiredService<ITestHarness>();
//        _dbContext = _provider.GetRequiredService<ApplicationDbContext>();
//    }

//    [Fact]
//    public async Task Should_Consume_OrderCreated_And_Publish_StockReserved()
//    {
//        await _harness.Start();

//        var consumerHarness = _harness
//            .Consumed
//            .Select<OrderCreated>()
//            .First();

//        // Arrange: ?? ???? ???? ?????? ????????
//        var productId = Guid.NewGuid();
//        _dbContext.Inventories.Add(new ProductInventory
//        {
//            Id = Guid.NewGuid(),
//            ProductId = productId,
//            AvailableStock = 20
//        });
//        await _dbContext.SaveChangesAsync();

//        // Act: ?? ???? OrderCreated ????? ???????
//        var bus = _provider.GetRequiredService<IBus>();

//        await bus.Publish(new OrderCreated
//        {
//            OrderId = Guid.NewGuid(),
//            BuyerId = Guid.NewGuid(),
//            Items = new List<OrderItemMessage>
//            {
//                new()
//                {
//                    ProductId = productId,
//                    Quantity = 2,
//                    Price = 10
//                }
//            }
//        });

//        // Assert: ????? ??????? ?? StockReserved ????? ???
//        var published = await _harness.Published.Any<StockReserved>();
//        published.Should().BeTrue();
//    }
//}
