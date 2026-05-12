namespace ECommerceApi.Domain.Entities;

/// <summary>
/// Localized text content for a SwiperSlide.
/// Each slide supports multiple languages.
/// </summary>
public class SwiperSlideTranslation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SwiperSlideId { get; set; }
    public SwiperSlide SwiperSlide { get; set; } = null!;

    public string LanguageCode { get; set; } = string.Empty;

    // Four text fields as per spec
    public string TopText { get; set; } = string.Empty;           // required
    public string BigTitle { get; set; } = string.Empty;          // required
    public string HighlightedTitleNormal { get; set; } = string.Empty;   // required (highlight 1)
    public string HighlightedTitleColor { get; set; } = string.Empty;    // required (highlight 2 - colored)
    public string? HighlightedTitleBold { get; set; }             // optional  (highlight 3 - bold)
    public string BottomText { get; set; } = string.Empty;        // required
}
