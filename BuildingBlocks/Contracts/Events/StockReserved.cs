namespace BuildingBlocks.Contracts.Events;

public class StockReserved
{
    public Guid OrderId { get; set; }
    public decimal Total { get; set; }
}
