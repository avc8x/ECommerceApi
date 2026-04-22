using ECommerceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApi.Infrastructure.Persistence.Configurations;

public class SwiperSlideTranslationConfiguration : IEntityTypeConfiguration<SwiperSlideTranslation>
{
    public void Configure(EntityTypeBuilder<SwiperSlideTranslation> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.LanguageCode).IsRequired().HasMaxLength(10);
        builder.Property(t => t.TopText).IsRequired().HasMaxLength(300);
        builder.Property(t => t.BigTitle).IsRequired().HasMaxLength(500);
        builder.Property(t => t.HighlightedTitleNormal).IsRequired().HasMaxLength(300);
        builder.Property(t => t.HighlightedTitleColor).IsRequired().HasMaxLength(300);
        builder.Property(t => t.HighlightedTitleBold).HasMaxLength(300);
        builder.Property(t => t.BottomText).IsRequired().HasMaxLength(500);

        builder.HasIndex(t => new { t.SwiperSlideId, t.LanguageCode }).IsUnique();
    }
}
