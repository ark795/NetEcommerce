using BasketService.API.Application.Interfaces;
using BasketService.API.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace BasketService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }
    [HttpGet("{userId}")]
    public async Task<IActionResult> Get(string userId)
    {
        var basket = await _basketService.GetBasketAsync(userId);
        return Ok(basket);
    }
    [HttpPost]
    public async Task<IActionResult> Update([FromBody] Basket basket)
    {
        var updated = await _basketService.UpdateBasketAsync(basket);
        return Ok(updated);
    }
    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(string userId)
    {
        await _basketService.DeleteBasketAsync(userId);
        return NoContent();
    }
}