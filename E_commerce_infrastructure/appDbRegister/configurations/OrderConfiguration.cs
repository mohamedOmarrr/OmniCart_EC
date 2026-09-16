using E_commerce_domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_infrastructure.configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.ToTable("Orders");

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.DeliveryMethodName)
            .IsRequired()
            .HasMaxLength(Order.MaxDeliveryMethodNameLength);

        builder.Property(x => x.ShippingCost)
            .HasPrecision(18, 2);

        builder.Property(x => x.DeliveryMethodEstimatedTime)
            .IsRequired()
            .HasMaxLength(Order.MaxDeliveryTimeLength);

        builder.Property(x => x.ShippingRecipientFullname)
            .IsRequired()
            .HasMaxLength(Order.MaxNameLength);



        builder.Property(x => x.ShippingMainPhoneNumber)
            .IsRequired()
            .HasMaxLength(Order.MaxPhoneLength);
        
        builder.Property(x => x.ShippingStepPhoneNumber)
            .IsRequired()
            .HasMaxLength(Order.MaxPhoneLength);

        builder.Property(x => x.ShippingAddress)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.SubTotal).HasPrecision(18, 2);
        builder.Property(x => x.ShippingCost).HasPrecision(18, 2);
        builder.Property(x => x.Total).HasPrecision(18, 2);

        builder.Property(x => x.PaymentTransactionId)
            .HasMaxLength(200);

        builder.HasIndex(x => x.PaymentTransactionId);

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Items)
            .HasField("_items")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CreatedAt);
    }
}