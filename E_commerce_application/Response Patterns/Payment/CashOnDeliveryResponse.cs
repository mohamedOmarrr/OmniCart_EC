using E_commerce_application.DTOs;
using E_commerce_application.Response_Patterns.Payment;

namespace E_commerce_infrastructure.Identities;

public record CashOnDeliveryResponse(
    Guid OrderId,
    OrderReceiptDto Receipt
) : BaseOrderResponse;