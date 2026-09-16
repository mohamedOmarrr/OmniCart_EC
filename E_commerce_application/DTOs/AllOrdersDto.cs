namespace E_commerce_application.DTOs;

public record AllOrdersDto(
        Guid OrderId,
        Guid CustomerId,
        string OrderStatus,
        string PaymentMethod,
        string DeliveryMethodName,
        string CustomerFullName,
        string MainPhoneNumber,
        decimal TotalPrice,
        DateTimeOffset CreatedAt
    );