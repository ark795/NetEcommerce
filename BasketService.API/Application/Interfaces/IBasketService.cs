using BasketService.API.Domain.Entities;
namespace BasketService.API.Application.Interfaces;
public interface IBasketService
{
    Task<Basket> GetBasketAsync(string userId);
    Task<Basket> UpdateBasketAsync(Basket basket);
    Task DeleteBasketAsync(string userId);
}