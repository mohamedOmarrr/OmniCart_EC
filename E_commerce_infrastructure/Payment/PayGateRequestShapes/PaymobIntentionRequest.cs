using System.Text.Json.Serialization;

namespace E_commerce_infrastructure.Payment;

public record PaymobIntentionRequest(
    [property: JsonPropertyName("amount")] int Amount,

    [property: JsonPropertyName("currency")] string Currency,

    [property: JsonPropertyName("payment_methods")]
    List<int> PaymentMethods,

    [property: JsonPropertyName("items")]
    List<PaymobItem> Items,

    [property: JsonPropertyName("billing_data")]
    PaymobBillingData BillingData,

    [property: JsonPropertyName("special_reference")]
    string? SpecialReference,

    [property: JsonPropertyName("notification_url")]
    string? NotificationUrl,

    [property: JsonPropertyName("redirection_url")]
    string? RedirectionUrl);