using BuildingBlocks.Contracts.Events;
using MassTransit;
using PaymentService.API.Application.Interfaces;
namespace PaymentService.API.Consumers;
public class StockReservedConsumer : IConsumer<StockReserved>
{
    private readonly IPaymentProcessor _paymentProcessor;
    public StockReservedConsumer(IPaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor;
    }
    public async Task Consume(ConsumeContext<StockReserved> context)
    {
        var success = await _paymentProcessor.ProcessPaymentAsync(context.Message.OrderId, context.Message.Total);
        if (success)
        {
            await context.Publish(new PaymentSucceeded
            {
                OrderId = context.Message.OrderId
            });
        }
        else
        {
            await context.Publish(new PaymentFailed
            {
                OrderId = context.Message.OrderId,
                Reason = "Payment declined by bank"
            });
        }
    }
}