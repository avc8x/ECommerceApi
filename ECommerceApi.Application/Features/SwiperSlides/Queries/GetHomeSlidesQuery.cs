using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Application.Features.SwiperSlides.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.SwiperSlides.Queries;

public record GetHomeSlidesQuery(string? LanguageCode = null) : IRequest<Result<IEnumerable<SwiperSlideDto>>>;

public class GetHomeSlidesQueryHandler : IRequestHandler<GetHomeSlidesQuery, Result<IEnumerable<SwiperSlideDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public GetHomeSlidesQueryHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<IEnumerable<SwiperSlideDto>>> Handle(GetHomeSlidesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "home-slides:admin";
        var cached = await _cache.GetAsync<IEnumerable<SwiperSlideDto>>(cacheKey, cancellationToken);
        if (cached is not null) return Result<IEnumerable<SwiperSlideDto>>.Success(cached);

        var slides = await _context.SwiperSlides
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.DisplayOrder)
            .Include(s => s.Translations)
            .Include(s => s.Category)
            .ToListAsync(cancellationToken);

        var result = slides.Select(s => new SwiperSlideDto(
            s.Id,
            s.ImageUrl,
            s.DisplayOrder,
            s.CategoryId,
            s.Category.Slug,
            s.Translations.Select(t => new SwiperSlideTranslationDto(
                t.LanguageCode, t.TopText, t.BigTitle,
                t.HighlightedTitleNormal, t.HighlightedTitleColor,
                t.HighlightedTitleBold, t.BottomText))
        )).ToList();

        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10), cancellationToken);
        return Result<IEnumerable<SwiperSlideDto>>.Success(result);
    }
}
