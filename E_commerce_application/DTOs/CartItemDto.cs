namespace E_commerce_application.DTOs;

public record CartItemDto(
        Guid ProductId,
        string ProductName,
        string ImageUrl,
        decimal UnitPrice,
        int Quantity,
        decimal LineTotal
    );