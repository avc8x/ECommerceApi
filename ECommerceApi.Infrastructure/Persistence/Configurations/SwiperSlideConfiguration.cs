using ECommerceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApi.Infrastructure.Persistence.Configurations;

public class SwiperSlideConfiguration : IEntityTypeConfiguration<SwiperSlide>
{
    public void Configure(EntityTypeBuilder<SwiperSlide> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.ImageUrl).IsRequired().HasMaxLength(1000);

        builder.HasOne(s => s.Category)
            .WithMany()
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Translations)
            .WithOne(t => t.SwiperSlide)
            .HasForeignKey(t => t.SwiperSlideId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}
