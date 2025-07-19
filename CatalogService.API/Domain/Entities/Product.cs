using Microsoft.AspNetCore.Http.HttpResults;

namespace CatalogService.API.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public string ImageUrl { get; private set; }
    public string Brand { get; private set; }
    public string Category { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Product(string name, string description, decimal price, int stockQuantity, string imageUrl, string brand, string category)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        ImageUrl = imageUrl;
        Brand = brand;
        Category = category;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void UpdateStock(int quantity) => StockQuantity = quantity;
    public void Deactivate() => IsActive = false;
}