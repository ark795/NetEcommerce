using InventoryService.API.Application.Interfaces;
using InventoryService.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace InventoryService.API.Infrastructure.Repositories;
public class InventoryServiceImpl : IInventoryService
{
    private readonly ApplicationDbContext _context;
    public InventoryServiceImpl(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> ReserveStockAsync(List<(Guid ProductId, int Quantity)> items)
    {
        foreach (var item in items)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == item.ProductId);
            if (inventory == null || inventory.AvailableStock < item.Quantity)
                return false;
            inventory.AvailableStock -= item.Quantity;
        }
        await _context.SaveChangesAsync();
        return true;
    }
}