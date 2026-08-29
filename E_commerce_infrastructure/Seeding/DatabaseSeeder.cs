namespace E_commerce_infrastructure.Seeding;

public class DatabaseSeeder(IEnumerable<IDataSeeder> seeders, AppDbContext dbContext)
{
    public async Task SeedAllAsync(CancellationToken ct = default)
    {
        var sortedSeeders = seeders.OrderBy(x => x.Order);
        foreach (var seeder in sortedSeeders)
        {
            await seeder.SeedAsync(ct);
        }
    }
}