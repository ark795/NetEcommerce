using OrderService.API.Application.Interfaces;
using OrderService.API.Domain.Entities;
using OrderService.API.Infrastructure.Persistence;
namespace OrderService.API.Infrastructure.Repositories;
public class OrderServiceImpl : IOrderService
{
    private readonly ApplicationDbContext _context;
    public OrderServiceImpl(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task CreateOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
}