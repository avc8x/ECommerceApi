using System.Text;
using System.Text.RegularExpressions;
using ECommerceApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using ECommerceApi.Infrastructure.Persistence;

namespace ECommerceApi.Infrastructure.Services;

public class SlugService : ISlugService
{
    private readonly AppDbContext _context;

    public SlugService(AppDbContext context)
    {
        _context = context;
    }

    public string GenerateSlug(string text)
    {
        // Normalize unicode -> remove diacritics
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            var category = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != System.Globalization.UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        var slug = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"[\s-]+", "-").Trim('-');
        return slug;
    }

    public async Task<string> EnsureUniqueSlugAsync(string baseSlug, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var slug = baseSlug;
        var counter = 1;

        while (true)
        {
            var query = _context.Categories.Where(c => c.Slug == slug);
            if (excludeId.HasValue) query = query.Where(c => c.Id != excludeId.Value);
            if (!await query.AnyAsync(cancellationToken)) return slug;

            slug = $"{baseSlug}-{counter++}";
        }
    }
}
