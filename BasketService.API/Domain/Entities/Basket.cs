namespace BasketService.API.Domain.Entities;

public class Basket
{
    public string UserId { get; set; } = default!;
    public List<BasketItem> Items { get; set; }
}