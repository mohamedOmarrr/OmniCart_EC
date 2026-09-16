using E_commerce_application.Response_Patterns;
using E_commerce_domain.Entities;
using E_commerce_infrastructure.Payment;
using E_Commerce_persentation.HttpRequests.Payment;

namespace E_commerce_application.Interfaces;

public interface IPaymentService
{
    Task<PaymobPaymentResponse> CreatePaymentAsync(
        Guid orderId,
        decimal totalAmount,
        decimal deliveryPrice,
        IReadOnlyCollection<OrderItem> items,
        string firstName,
        string lastName,
        string email,
        CancellationToken cancellationToken);
    
    bool VerifyWebhook(
        PaymobTransaction transaction,
        string? hmac);
}