using OrderService.API.Domain.Entities;
namespace OrderService.API.Application.Interfaces;
public interface IOrderService
{
    Task CreateOrderAsync(Order order);
}