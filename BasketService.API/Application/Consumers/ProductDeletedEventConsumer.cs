using BasketService.API.Application.Interfaces;
using CatalogService.API.Contracts.Events;
using MassTransit;
namespace BasketService.API.Application.Consumers;
public class ProductDeletedEventConsumer : IConsumer<ProductDeletedEvent>
{
    private readonly ILogger<ProductDeletedEventConsumer> _logger;
    private readonly IBasketService _basketService;
    public ProductDeletedEventConsumer(
    ILogger<ProductDeletedEventConsumer> logger,
    IBasketService basketService)
    {
        _logger = logger;
        _basketService = basketService;
    }
    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received ProductDeletedEvent: {ProductId}", message.Id);
        var userId = "test-user";
        var basket = await _basketService.GetBasketAsync(userId);
        if (basket.Items != null)
        {
            var removedCount = basket.Items.RemoveAll(i => i.ProductId == message.Id);
            if (removedCount > 0)
            {
                await _basketService.UpdateBasketAsync(basket);
                _logger.LogInformation("Removed product {ProductId} from basket of user {UserId}", message.Id, userId);
            }
        }
    }
}