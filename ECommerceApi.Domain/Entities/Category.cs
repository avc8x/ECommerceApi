using ECommerceApi.Domain.Common;

namespace ECommerceApi.Domain.Entities;

/// <summary>
/// Represents a product category. Supports hierarchical nesting (category -> sub -> sub-sub).
/// Localization is handled via CategoryTranslation collection.
/// </summary>
public class Category : BaseEntity
{
    public string Slug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }

    // Self-referencing hierarchy
    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }
    public ICollection<Category> Children { get; set; } = new List<Category>();

    // Localization
    public ICollection<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();

    // Navigation to popular category entry (if this category is marked popular)
    public PopularCategory? PopularCategory { get; set; }
}
