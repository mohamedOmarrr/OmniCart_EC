using E_commerce_domain.Entities;
using E_commerce_infrastructure.Seeding.Data.Models;

namespace E_commerce_infrastructure.Seeding;

public class ProductSeeder(AppDbContext dbContext) : IDataSeeder
{
    public int Order => 5;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        await JsonSeeder.SeedIfEmpty<Product, ProductSeedModel>(
            dbContext.Products,
            "Product.json",
            p => Product.Create(
                p.Id,
                p.Name,
                p.Description,
                p.PictureUrl,
                p.Price,
                p.ProductBrandId,
                p.ProductCategoryId).Value,
            ct);

        await dbContext.SaveChangesAsync(ct);
    }
}