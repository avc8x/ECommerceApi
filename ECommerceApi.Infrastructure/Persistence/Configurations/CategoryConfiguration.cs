using ECommerceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApi.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(300);

        builder.HasIndex(c => c.Slug).IsUnique();

        builder.Property(c => c.ImageUrl).HasMaxLength(1000);

        // Self-referencing hierarchy
        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Soft delete: filter out deleted by default
        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasMany(c => c.Translations)
            .WithOne(t => t.Category)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
