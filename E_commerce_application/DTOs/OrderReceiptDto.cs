namespace E_commerce_application.DTOs;

public record OrderReceiptDto(
    IReadOnlyList<OrderItemDto> Items,
    decimal SubTotal,
    decimal ShippingCost,
    decimal Total,
    string OrderStatus,
    string DeliveryMethodName,
    string DeliveryMethodEstimatedTime,
    string ShippingMethodName,
    string ShippingRecipientFullname,
    string ShippingMainPhoneNumber,
    string ShippingStepPhoneNumber,
    string ShippingAddress
);