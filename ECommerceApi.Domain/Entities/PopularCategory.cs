using ECommerceApi.Domain.Common;

namespace ECommerceApi.Domain.Entities;

/// <summary>
/// Represents a selected top-level (level 1) category shown on the home page popular section.
/// Only non-nested categories can be added. Order is manually controlled.
/// </summary>
public class PopularCategory : BaseEntity
{
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    /// <summary>Display order on the home page (0-based, lower = first).</summary>
    public int DisplayOrder { get; set; }
}
