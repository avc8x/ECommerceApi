using ECommerceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApi.Infrastructure.Persistence.Configurations;

public class PopularCategoryConfiguration : IEntityTypeConfiguration<PopularCategory>
{
    public void Configure(EntityTypeBuilder<PopularCategory> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.CategoryId).IsUnique(); // one entry per category

        builder.HasOne(p => p.Category)
            .WithOne(c => c.PopularCategory)
            .HasForeignKey<PopularCategory>(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
