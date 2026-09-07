namespace E_Commerce_persentation.HttpRequests;

public record UpdateProductRequest(
    Guid Id,
    string? Name,
    string? Description,
    decimal? Price,
    IFormFile? Image
    );