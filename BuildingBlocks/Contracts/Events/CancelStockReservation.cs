using BuildingBlocks.Contracts.Dtos;

namespace BuildingBlocks.Contracts.Events;

public class CancelStockReservation
{
    public Guid OrderId { get; set; }
    public List<OrderItemMessage> Items { get; set; } = new();
}
