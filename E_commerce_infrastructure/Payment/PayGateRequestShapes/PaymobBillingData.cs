using System.Text.Json.Serialization;

namespace E_commerce_infrastructure.Payment;

public record PaymobBillingData(
    [property: JsonPropertyName("first_name")] string FirstName,

    [property: JsonPropertyName("last_name")] string LastName,

    [property: JsonPropertyName("email")] string Email);