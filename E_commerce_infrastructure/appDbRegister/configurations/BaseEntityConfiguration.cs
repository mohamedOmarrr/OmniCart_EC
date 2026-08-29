using E_commerce_domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_infrastructure.configurations;

internal class BaseEntityConfiguration
{
    public static void Configure<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity
    {
        builder.Property(entity => entity.IsDeleted)
            .HasDefaultValue(false);

        builder.HasQueryFilter(entity => !entity.IsDeleted);
    }
}
