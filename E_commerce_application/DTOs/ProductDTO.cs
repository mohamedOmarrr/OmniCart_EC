namespace E_commerce_application.DTOs;

public record ProductDTO(
        Guid Id,
        string Name,
        decimal Price,
        string Description,
        string ImageUrl
    );