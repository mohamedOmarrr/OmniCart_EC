using E_commerce_application.Response_Patterns.Payment;

namespace E_commerce_application.Response_Patterns;

public record CardPaymentResponse(
    Guid OrderId,
    string TransactionId,
    string ClientSecret
) : BaseOrderResponse;