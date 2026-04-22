using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Application.Features.Categories.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.Categories.Queries;

/// <summary>Client query — returns tree localized to the requested language.</summary>
public record GetCategoryTreeByLangQuery(string LanguageCode) : IRequest<Result<IEnumerable<CategoryTreeDto>>>;

public class GetCategoryTreeByLangQueryHandler : IRequestHandler<GetCategoryTreeByLangQuery, Result<IEnumerable<CategoryTreeDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public GetCategoryTreeByLangQueryHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<IEnumerable<CategoryTreeDto>>> Handle(GetCategoryTreeByLangQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"categories:client:{request.LanguageCode}";
        var cached = await _cache.GetAsync<IEnumerable<CategoryTreeDto>>(cacheKey, cancellationToken);
        if (cached is not null) return Result<IEnumerable<CategoryTreeDto>>.Success(cached);

        var allCategories = await _context.Categories
            .Where(c => !c.IsDeleted)
            .Include(c => c.Translations)
            .ToListAsync(cancellationToken);

        var roots = allCategories.Where(c => c.ParentId == null).ToList();
        var tree = roots.Select(c => MapToDto(c, allCategories, request.LanguageCode))
            .OrderBy(c => c.Title)
            .ToList();

        await _cache.SetAsync(cacheKey, tree, TimeSpan.FromMinutes(10), cancellationToken);
        return Result<IEnumerable<CategoryTreeDto>>.Success(tree);
    }

    private static CategoryTreeDto MapToDto(Domain.Entities.Category cat, List<Domain.Entities.Category> all, string lang)
    {
        var translation = cat.Translations.FirstOrDefault(t => t.LanguageCode == lang)
            ?? cat.Translations.FirstOrDefault();

        var children = all.Where(c => c.ParentId == cat.Id)
            .Select(c => MapToDto(c, all, lang))
            .OrderBy(c => c.Title);

        return new CategoryTreeDto(
            cat.Id,
            cat.Slug,
            translation?.Title ?? cat.Slug,
            translation?.Description,
            cat.ImageUrl,
            cat.ParentId,
            children
        );
    }
}
