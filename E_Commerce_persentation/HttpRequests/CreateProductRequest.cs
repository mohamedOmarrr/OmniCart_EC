namespace E_Commerce_persentation.HttpRequests;

public class CreateProductRequest
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public IFormFile Image { get; set; }

    public string CategoryName { get; set; } = null!;
    public string BrandName { get; set; } = null!;
}