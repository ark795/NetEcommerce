namespace InventoryService.API.Application.Interfaces;
public interface IInventoryService
{
    Task<bool> ReserveStockAsync(List<(Guid ProductId, int Quantity)> items);
}