using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Application.Features.SwiperSlides.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.SwiperSlides.Queries;

public record GetHomeSlidesByLangQuery(string LanguageCode) : IRequest<Result<IEnumerable<SwiperSlideClientDto>>>;

public class GetHomeSlidesByLangQueryHandler : IRequestHandler<GetHomeSlidesByLangQuery, Result<IEnumerable<SwiperSlideClientDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public GetHomeSlidesByLangQueryHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<IEnumerable<SwiperSlideClientDto>>> Handle(GetHomeSlidesByLangQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"home-slides:{request.LanguageCode}";
        var cached = await _cache.GetAsync<IEnumerable<SwiperSlideClientDto>>(cacheKey, cancellationToken);
        if (cached is not null) return Result<IEnumerable<SwiperSlideClientDto>>.Success(cached);

        var slides = await _context.SwiperSlides
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.DisplayOrder)
            .Include(s => s.Translations)
            .Include(s => s.Category)
            .ToListAsync(cancellationToken);

        var result = slides.Select(s =>
        {
            var t = s.Translations.FirstOrDefault(t => t.LanguageCode == request.LanguageCode)
                ?? s.Translations.FirstOrDefault();
            return new SwiperSlideClientDto(
                s.Id, s.ImageUrl, s.DisplayOrder, s.CategoryId, s.Category.Slug,
                t?.TopText ?? "", t?.BigTitle ?? "",
                t?.HighlightedTitleNormal ?? "", t?.HighlightedTitleColor ?? "",
                t?.HighlightedTitleBold, t?.BottomText ?? "");
        }).ToList();

        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10), cancellationToken);
        return Result<IEnumerable<SwiperSlideClientDto>>.Success(result);
    }
}
