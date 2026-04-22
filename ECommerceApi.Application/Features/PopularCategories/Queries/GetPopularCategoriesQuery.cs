using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Application.Features.PopularCategories.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.PopularCategories.Queries;

public record GetPopularCategoriesQuery(string? LanguageCode = null) : IRequest<Result<IEnumerable<PopularCategoryDto>>>;

public class GetPopularCategoriesQueryHandler : IRequestHandler<GetPopularCategoriesQuery, Result<IEnumerable<PopularCategoryDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public GetPopularCategoriesQueryHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<IEnumerable<PopularCategoryDto>>> Handle(GetPopularCategoriesQuery request, CancellationToken cancellationToken)
    {
        var lang = request.LanguageCode ?? "en";
        var cacheKey = $"popular-categories:{lang}";
        var cached = await _cache.GetAsync<IEnumerable<PopularCategoryDto>>(cacheKey, cancellationToken);
        if (cached is not null) return Result<IEnumerable<PopularCategoryDto>>.Success(cached);

        var items = await _context.PopularCategories
            .Where(p => !p.IsDeleted)
            .OrderBy(p => p.DisplayOrder)
            .Include(p => p.Category)
                .ThenInclude(c => c.Translations)
            .ToListAsync(cancellationToken);

        var result = items.Select(p =>
        {
            var translation = p.Category.Translations.FirstOrDefault(t => t.LanguageCode == lang)
                ?? p.Category.Translations.FirstOrDefault();
            return new PopularCategoryDto(
                p.Id,
                p.CategoryId,
                p.Category.Slug,
                translation?.Title ?? p.Category.Slug,
                p.Category.ImageUrl,
                p.DisplayOrder
            );
        }).ToList();

        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10), cancellationToken);
        return Result<IEnumerable<PopularCategoryDto>>.Success(result);
    }
}
