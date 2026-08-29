using E_commerce_domain.Entities;
using E_commerce_infrastructure.Seeding.Data.Models;

namespace E_commerce_infrastructure.Seeding;

public class CategorySeeder(AppDbContext dbContext) : IDataSeeder
{
        public int Order => 3;

        public async Task SeedAsync(CancellationToken ct = default)
        {
                await JsonSeeder.SeedIfEmpty<Category, CategorySeedModel>
                    (dbContext.Categories, "Category.json", b => Category.Create(b.Id, b.Name).Value, ct);
                await dbContext.SaveChangesAsync(ct);   
        }
}