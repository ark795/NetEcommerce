using BuildingBlocks.Contracts.Events;
using InventoryService.API.Application.Interfaces;
using MassTransit;
namespace InventoryService.API.Infrastructure.Consumers;
public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<OrderCreatedConsumer> _logger;
    public OrderCreatedConsumer(IInventoryService inventoryService, ILogger<OrderCreatedConsumer> logger)
    {
        _inventoryService = inventoryService;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        try
        {
            _logger.LogInformation($"Consuming OrderCreated message for OrderId: {context.Message.OrderId}");
            var success = await _inventoryService.ReserveStockAsync(
            context.Message.Items.Select(i => (i.ProductId, i.Quantity)).ToList());
            if (success)
            {
                _logger.LogInformation($"Stock reserved successfully for OrderId: {context.Message.OrderId}");
                await context.Publish(new StockReserved
                {
                    OrderId = context.Message.OrderId,
                    Total = context.Message.Items.Sum(i => i.Quantity * i.Price)
                });
            }
            else
            {
                _logger.LogWarning($"Not enough stock for OrderId: {context.Message.OrderId}. Sending PaymentFailed event.");
                await context.Publish(new PaymentFailed
                {
                    OrderId = context.Message.OrderId,
                    Reason = "Not enough stock"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while processing OrderCreated for OrderId: {context.Message.OrderId}");
            await context.Publish(new PaymentFailed
            {
                OrderId = context.Message.OrderId,
                Reason = "Internal server error"
            });
        }
    }
}