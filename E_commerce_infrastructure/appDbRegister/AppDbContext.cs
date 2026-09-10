using E_commerce_domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<OrderShippingDetails> OrderShippingDetails => Set<OrderShippingDetails>();
    public DbSet<DeliveryMethod> DeliveryMethods => Set<DeliveryMethod>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();
    public DbSet<WishItem> WishItems => Set<WishItem>();
    
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly,
            type => type.Namespace == "ECommerce.Infrastructure.Persistence.Configurations");

        base.OnModelCreating(modelBuilder);
    }
}