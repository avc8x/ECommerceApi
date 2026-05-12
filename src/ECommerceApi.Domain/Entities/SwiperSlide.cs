using ECommerceApi.Domain.Common;

namespace ECommerceApi.Domain.Entities;

/// <summary>
/// Represents a hero banner slide on the home page.
/// Slides are manually ordered. Each slide links to a category.
/// Text content is localized via SwiperSlideTranslation.
/// </summary>
public class SwiperSlide : BaseEntity
{
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>Display order (lower = first).</summary>
    public int DisplayOrder { get; set; }

    /// <summary>Link to any category (any level).</summary>
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<SwiperSlideTranslation> Translations { get; set; } = new List<SwiperSlideTranslation>();
}
