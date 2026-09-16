using E_commerce_infrastructure.Identities;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_infrastructure.Seeding;

public class DatabaseSeeder(IEnumerable<IDataSeeder> seeders, AppDbContext dbContext, AppIdentityDbContext identityDbContext)
{
    public async Task SeedAllAsync(CancellationToken ct = default)
    {
        await dbContext.Database.MigrateAsync(ct);
        await identityDbContext.Database.MigrateAsync(ct);
        
        var sortedSeeders = seeders.OrderBy(x => x.Order);
        foreach (var seeder in sortedSeeders)
        {
            await seeder.SeedAsync(ct);
        }
    }
}