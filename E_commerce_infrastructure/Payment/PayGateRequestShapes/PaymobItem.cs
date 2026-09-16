using System.Text.Json.Serialization;

namespace E_commerce_infrastructure.Payment;

public record PaymobItem(
    [property: JsonPropertyName("name")] string Name,

    [property: JsonPropertyName("amount")] int Amount,

    [property: JsonPropertyName("quantity")] int Quantity);