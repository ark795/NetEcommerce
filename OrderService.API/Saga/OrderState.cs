using MassTransit;
namespace OrderService.API.Saga;
public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public Guid BuyerId { get; set; }
    public DateTime Created { get; set; }
}