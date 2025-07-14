using BuildingBlocks.Contracts.Events;
using MassTransit;
namespace OrderService.API.Saga;
public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State StockReserved { get; private set; }
    public State PaymentCompleted { get; private set; }
    public State OrderCompleted { get; private set; }
    public State Failed { get; private set; }
    public Event<OrderCreated> OrderCreatedEvent { get; private set; }
    public Event<StockReserved> StockReservedEvent { get; private set; }
    public Event<PaymentSucceeded> PaymentSucceededEvent { get; private set; }
    public Event<PaymentFailed> PaymentFailedEvent { get; private set; }
    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);
        Event(() => OrderCreatedEvent, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => StockReservedEvent, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => PaymentSucceededEvent, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => PaymentFailedEvent, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Initially(
        When(OrderCreatedEvent)
        .Then(ctx =>
        {
            ctx.Instance.OrderId = ctx.Data.OrderId;
            ctx.Instance.BuyerId = ctx.Data.BuyerId;
            ctx.Instance.Created = DateTime.UtcNow;
        })
        .TransitionTo(StockReserved)
        .Publish(ctx => new ReserveStock
        {
            OrderId = ctx.Data.OrderId,
            Items = ctx.Data.Items
        })
        );
        During(StockReserved,
        When(StockReservedEvent)
        .TransitionTo(PaymentCompleted)
        .Publish(ctx => new ProcessPayment
        {
            OrderId = ctx.Data.OrderId,
            Amount = ctx.Data.Total
        }),
        When(PaymentFailedEvent)
        .TransitionTo(Failed)
        .Publish(ctx => new CancelStockReservation
        {
            OrderId = ctx.Data.OrderId,
            //Items = ctx.Instance.OrderItems
        })
        );
        During(PaymentCompleted,
        When(PaymentSucceededEvent)
        .TransitionTo(OrderCompleted)
        .Publish(ctx => new FinalizeOrder
        {
            OrderId = ctx.Data.OrderId
        })
        );
    }
}