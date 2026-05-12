using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Application.Features.Categories.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.Categories.Queries;

/// <summary>Admin query — returns full tree with all translations.</summary>
public record GetCategoriesTreeQuery : IRequest<Result<IEnumerable<CategoryDto>>>;

public class GetCategoriesTreeQueryHandler : IRequestHandler<GetCategoriesTreeQuery, Result<IEnumerable<CategoryDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;
    private const string CacheKey = "categories:admin:tree";

    public GetCategoriesTreeQueryHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetCategoriesTreeQuery request, CancellationToken cancellationToken)
    {
        var cached = await _cache.GetAsync<IEnumerable<CategoryDto>>(CacheKey, cancellationToken);
        if (cached is not null) return Result<IEnumerable<CategoryDto>>.Success(cached);

        var allCategories = await _context.Categories
            .Where(c => !c.IsDeleted)
            .Include(c => c.Translations)
            .ToListAsync(cancellationToken);

        // Build tree from flat list
        var roots = allCategories.Where(c => c.ParentId == null).OrderBy(c =>
            c.Translations.FirstOrDefault()?.Title ?? c.Slug).ToList();

        var tree = roots.Select(c => MapToDto(c, allCategories)).ToList();

        await _cache.SetAsync(CacheKey, tree, TimeSpan.FromMinutes(10), cancellationToken);
        return Result<IEnumerable<CategoryDto>>.Success(tree);
    }

    private static CategoryDto MapToDto(Domain.Entities.Category cat, List<Domain.Entities.Category> all)
    {
        var children = all.Where(c => c.ParentId == cat.Id)
            .OrderBy(c => c.Translations.FirstOrDefault()?.Title ?? c.Slug)
            .Select(c => MapToDto(c, all));

        return new CategoryDto(
            cat.Id,
            cat.Slug,
            cat.ImageUrl,
            cat.SortOrder,
            cat.ParentId,
            cat.Translations.Select(t => new CategoryTranslationDto(t.LanguageCode, t.Title, t.Description, t.MetaTitle, t.MetaDescription)),
            children
        );
    }
}
