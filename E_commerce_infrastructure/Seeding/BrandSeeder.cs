using E_commerce_domain.Entities;
using E_commerce_infrastructure.Seeding.Data.Models;

namespace E_commerce_infrastructure.Seeding;

public class BrandSeeder(AppDbContext dbContext) : IDataSeeder
{
    public int Order => 4;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        await JsonSeeder.SeedIfEmpty<Brand, BrandSeedModel>(
            dbContext.Brands,
            "Brand.json",
            b => Brand.Create(b.Id, b.Name).Value,
            ct);

        await dbContext.SaveChangesAsync(ct);
    }
}