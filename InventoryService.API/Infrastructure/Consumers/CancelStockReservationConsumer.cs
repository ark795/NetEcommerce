using BuildingBlocks.Contracts.Events;
using InventoryService.API.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
namespace InventoryService.API.Infrastructure.Consumers;
public class CancelStockReservationConsumer : IConsumer<CancelStockReservation>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CancelStockReservationConsumer> _logger;
    public CancelStockReservationConsumer(ApplicationDbContext context, ILogger<CancelStockReservationConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<CancelStockReservation> context)
    {
        try
        {
            _logger.LogInformation($"Reverting stock for OrderId: {context.Message.OrderId}");
            // فرض کنیم باید موجودی‌ها را برگردانیم 
            var orderItems = context.Message.Items;
            // برای هر آیتم، موجودی را برگردانیم (این قسمت را با منطق واقعی کامل کنید) 
            foreach (var item in orderItems)
            {
                var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == item.ProductId);
                if (inventory != null)
                {
                    inventory.AvailableStock += item.Quantity;
                    _context.Update(inventory);
                }
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Stock successfully reverted for OrderId: {context.Message.OrderId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while canceling stock reservation for OrderId: {context.Message.OrderId}");
        }
    }
}