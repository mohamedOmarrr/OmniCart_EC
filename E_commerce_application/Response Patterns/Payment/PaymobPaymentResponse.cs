using E_commerce_infrastructure.Payment;

namespace E_commerce_application.Response_Patterns;

public record PaymobPaymentResponse(
        bool IsSuccess,
        PaymobIntentionResponse? Response,
        string? ErrorMessage
    );