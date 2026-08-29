namespace E_commerce_infrastructure.Seeding.Data.Models;

public record ProductSeedModel(Guid Id, string Name, string Description, string PictureUrl, decimal Price, Guid ProductBrandId, Guid ProductCategoryId);
