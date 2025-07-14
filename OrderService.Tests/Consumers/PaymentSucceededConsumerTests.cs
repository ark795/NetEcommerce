//using BuildingBlocks.Contracts.Events;
//using MassTransit;
//using MassTransit.Testing;

//namespace OrderService.Tests.Consumers;

//public class PaymentSucceededConsumerTests : IClassFixture<OrderServiceAppFactory>
//{
//    private readonly OrderServiceAppFactory _factory;

//    public PaymentSucceededConsumerTests(OrderServiceAppFactory factory) => _factory = factory;

//    [Fact]
//    public async Task Should_Consume_PaymentSucceeded_And_Finalize_Order()
//    {
//        var harness = _factory.Services.GetRequiredService<ITestHarness>();
//        var producer = _factory.Services.GetRequiredService<IPublishEndpoint>();

//        var orderId = Guid.NewGuid();
//        await producer.Publish(new PaymentSucceeded { OrderId = orderId });

//        Assert.True(await harness.Consumed.Any<PaymentSucceeded>());
//    }
//}
