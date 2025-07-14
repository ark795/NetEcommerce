namespace InventoryService.API.Domain.Entities;
public class ProductInventory
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int AvailableStock { get; set; }
}