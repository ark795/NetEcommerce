using BasketService.API.Application.Interfaces;
using CatalogService.API.Contracts.Events;
using MassTransit;
namespace BasketService.API.Application.Consumers;
public class ProductUpdatedEventConsumer : IConsumer<ProductUpdatedEvent>
{
    private readonly ILogger<ProductUpdatedEventConsumer> _logger;
    private readonly IBasketService _basketService;
    public ProductUpdatedEventConsumer(
    ILogger<ProductUpdatedEventConsumer> logger,
    IBasketService basketService)
    {
        _logger = logger;
        _basketService = basketService;
    }
    public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received ProductUpdatedEvent: {Name} - {Price}", message.Name, message.Price);
        var userId = "test-user";
        var basket = await _basketService.GetBasketAsync(userId);
        if (basket.Items != null)
        {
            bool updated = false;
            foreach (var item in basket.Items)
            {
                if (item.ProductId == message.Id)
                {
                    item.ProductName = message.Name;
                    item.Price = message.Price;
                    updated = true;
                }
            }
            if (updated)
            {
                await _basketService.UpdateBasketAsync(basket);
                _logger.LogInformation("Basket for user {UserId} updated for product {ProductId}", userId, message.Id);
            }
        }
    }
}