using PaymentService.API.Application.Interfaces;
namespace PaymentService.API.Infrastructure.Services;
public class FakePaymentProcessor : IPaymentProcessor
{
    public async Task<bool> ProcessPaymentAsync(Guid orderId, decimal amount)
    {
        await Task.Delay(500); // simulate payment delay 
        return amount < 1000; // simulate success if under 1000 
    }
}