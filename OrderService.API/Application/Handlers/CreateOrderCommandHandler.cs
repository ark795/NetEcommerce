using MediatR;
using OrderService.API.Application.Commands;
using OrderService.API.Application.Interfaces;
using OrderService.API.Domain.Entities;
using OrderService.API.Domain.Enums;
using OrderService.API.Domain.ValueObjects;
namespace OrderService.API.Application.Handlers;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderService _orderService;
    public CreateOrderCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var address = new Address
        {
            Province = request.Province,
            City = request.City,
            Street = request.Street,
            ZipCode = request.ZipCode
        };
        var items = request.Items.Select(i => new OrderItem
        {
            Id = Guid.NewGuid(),
            ProductId = i.ProductId,
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            Price = i.Price
        }).ToList();
        var order = new Order
        {
            Id = Guid.NewGuid(),
            BuyerId = request.CustomerId,
            Address = address,
            Status = OrderStatus.Pending,
            CreatedDate = DateTime.UtcNow,
            OrderItems = items
        };
        await _orderService.CreateOrderAsync(order);
        return order.Id;
    }
}