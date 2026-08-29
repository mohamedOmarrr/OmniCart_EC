using E_commerce_domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_infrastructure.configurations;

public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.ToTable("DeliveryMethods");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(DeliveryMethod.MaxNameLength);

        builder.Property(x => x.Description)
            .HasMaxLength(DeliveryMethod.MaxDescriptionLength);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2);

        builder.Property(x => x.EstimatedDeliveryTime)
            .IsRequired()
            .HasMaxLength(DeliveryMethod.MaxDeliveryTimeLength);

        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.DisplayOrder);
    }
}