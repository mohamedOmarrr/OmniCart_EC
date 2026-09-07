namespace E_commerce_application.DTOs;

public record ProductInDetailsDto(
    Guid Id,
    string Name,
    decimal Price,
    string Description,
    string ImageUrl,
    string BrandName,
    string CategoryName
    );