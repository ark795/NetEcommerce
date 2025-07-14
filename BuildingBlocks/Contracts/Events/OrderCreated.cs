using BuildingBlocks.Contracts.Dtos;

namespace BuildingBlocks.Contracts.Events;

public class OrderCreated
{
    public Guid OrderId { get; set; }
    public Guid BuyerId { get; set; }
    public List<OrderItemMessage> Items { get; set; } = new();
}
