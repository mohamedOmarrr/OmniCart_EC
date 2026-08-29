using E_commerce_domain.Entities;
using E_commerce_infrastructure.Seeding.Data.Models;

namespace E_commerce_infrastructure.Seeding;

public class ProductSeeder(AppDbContext dbContext) : IDataSeeder
{
    public int Order => 5;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        await JsonSeeder.SeedIfEmpty<Product, ProductSeedModel>
            (dbContext.Products, "brands.json", b => Product.Create(b.Id, b.Name, b.Description, b.PictureUrl, b.Price, b.ProductBrandId, b.ProductCategoryId).Value, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}