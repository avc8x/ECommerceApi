namespace ECommerceApi.Domain.Entities;

/// <summary>
/// Stores localized content for a Category per language code (e.g. "en", "id", "ar").
/// </summary>
public class CategoryTranslation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public string LanguageCode { get; set; } = string.Empty; // e.g. "en", "id"
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
}
