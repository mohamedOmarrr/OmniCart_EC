using E_commerce_domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_infrastructure.configurations;

public class WishItemConfiguration: IEntityTypeConfiguration<WishItem>
{
    public void Configure(EntityTypeBuilder<WishItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Wishlist)
            .WithMany(x => x.WishItems)
            .HasForeignKey(x => x.WishlistId)
            .HasPrincipalKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}