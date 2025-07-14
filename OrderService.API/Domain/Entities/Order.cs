using OrderService.API.Domain.Enums;
using OrderService.API.Domain.ValueObjects;

namespace OrderService.API.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid BuyerId { get; set; }
    public Address Address { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
}
//public class Order 
//{ 
// public Guid Id { get; private set; } 
// public string CustomerId { get; private set; } 
// public Address Address { get; set; } 
// public DateTime CreatedAt { get; private set; } 
// public OrderStatus Status { get; private set; } 
// private readonly List<OrderItem> _items = new(); 
// public IReadOnlyCollection<OrderItem> Items => _items; 
// public Order(string customerId) 
// { 
// Id = Guid.NewGuid(); 
// CustomerId = customerId; 
// CreatedAt = DateTime.UtcNow; 
// Status = OrderStatus.Created; 
// } 
// public void AddItem(Guid productId, int quantity, decimal price, string productName) 
// { 
// _items.Add(new OrderItem(productId, quantity, price, productName)); 
// } 
// public void MarkAsSubmitted() => Status = OrderStatus.Submitted; 
//} 