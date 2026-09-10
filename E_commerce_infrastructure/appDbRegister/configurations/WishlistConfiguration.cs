using E_commerce_domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_infrastructure.configurations;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.HasKey(x => x.UserId);

        builder
            .HasMany(x => x.WishItems)
            .WithOne(x => x.Wishlist)
            .HasForeignKey(x => x.WishlistId)
            .HasPrincipalKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}