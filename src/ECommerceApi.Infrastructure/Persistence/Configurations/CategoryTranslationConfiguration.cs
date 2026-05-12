using ECommerceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApi.Infrastructure.Persistence.Configurations;

public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
{
    public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.LanguageCode).IsRequired().HasMaxLength(10);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Description).HasMaxLength(1000);
        builder.Property(t => t.MetaTitle).HasMaxLength(200);
        builder.Property(t => t.MetaDescription).HasMaxLength(500);

        // One translation per category per language
        builder.HasIndex(t => new { t.CategoryId, t.LanguageCode }).IsUnique();
    }
}
