using E_commerce_domain.Entities;

namespace E_commerce_application.DTOs;

public record MyOrdersDto(
    Guid Id,
    OrderStatus Status,
    decimal TotalAmount,
    string PaymentMethod,
    string DeliveryMethodName,
    DateTimeOffset CreatedAt
);