namespace E_Commerce_persentation.HttpRequests;

public class UpdateProductRequest
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public IFormFile? Image { get; set; }
}