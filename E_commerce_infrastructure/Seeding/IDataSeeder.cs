namespace E_commerce_infrastructure.Seeding;

public interface IDataSeeder
{
    int Order { get; }

    Task SeedAsync(CancellationToken ct = default);
}