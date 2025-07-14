namespace BuildingBlocks.Contracts.Events;

public class ProcessPayment
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}
