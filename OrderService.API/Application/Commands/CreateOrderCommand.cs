using MediatR;
using OrderService.API.Application.Dtos;
namespace OrderService.API.Application.Commands;
public class CreateOrderCommand : IRequest<Guid>
{
    public Guid CustomerId { get; set; }
    public string Province { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public List<OrderItemDto> Items { get; set; }
}