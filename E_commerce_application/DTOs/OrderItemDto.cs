namespace E_commerce_application.DTOs;

public record OrderItemDto(
    Guid OrderId,
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal
);