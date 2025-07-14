using BuildingBlocks.Contracts.Dtos;

namespace BuildingBlocks.Contracts.Events;

public class ReserveStock
{
    public Guid OrderId { get; set; }
    public List<OrderItemMessage> Items { get; set; } = new();
}
