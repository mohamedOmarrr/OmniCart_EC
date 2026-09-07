namespace E_Commerce_persentation.HttpRequests;

public record CreateProductRequest(
        string Name,
        string Description,
        decimal Price,
        IFormFile Image,
        string CategoryName,
        string BrandName
    );