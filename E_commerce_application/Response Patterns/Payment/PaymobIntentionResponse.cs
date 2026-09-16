using System.Text.Json.Serialization;

namespace E_commerce_infrastructure.Payment;

public record PaymobIntentionResponse(
    [property: JsonPropertyName("intention_order_id")]
    int IntentionOrderId,

    [property: JsonPropertyName("id")]
    string Id,

    [property: JsonPropertyName("client_secret")]
    string ClientSecret);