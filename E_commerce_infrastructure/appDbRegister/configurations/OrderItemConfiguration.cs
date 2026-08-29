using E_commerce_domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_infrastructure.configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId).IsRequired();

           builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(200);

           builder.Property(p => p.PictureUrl)
               .IsRequired();
           

           builder.Property(p => p.UnitPrice)
                .HasColumnName("UnitPrice")
                .HasPrecision(18, 2);

           builder.HasIndex(p => p.ProductId);
           

        builder.HasIndex(x => x.OrderId);

        builder.Ignore(x => x.LineTotal);
    }
}