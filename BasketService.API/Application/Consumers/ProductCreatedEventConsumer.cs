using BasketService.API.Application.Interfaces;
using BasketService.API.Domain.Entities;
using CatalogService.API.Contracts.Events;
using MassTransit;
namespace BasketService.API.Application.Consumers;
public class ProductCreatedEventConsumer : IConsumer<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventConsumer> _logger;
    private readonly IBasketService _basketService;
    public ProductCreatedEventConsumer(
    ILogger<ProductCreatedEventConsumer> logger,
    IBasketService basketService)
    {
        _logger = logger;
        _basketService = basketService;
    }
    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received ProductCreatedEvent: {Name} - {Price}", message.Name, message.Price);
        var userId = "test-user";
        var basket = await _basketService.GetBasketAsync(userId);
        if (basket.Items == null)
            basket.Items = new List<BasketItem>();
        basket.Items.Add(new BasketItem
        {
            ProductId = message.Id,
            ProductName = message.Name,
            Price = message.Price,
            Quantity = 1
        });
        await _basketService.UpdateBasketAsync(basket);
    }
}