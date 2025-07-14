namespace PaymentService.API.Application.Interfaces;
public interface IPaymentProcessor
{
    Task<bool> ProcessPaymentAsync(Guid orderId, decimal amount);
}