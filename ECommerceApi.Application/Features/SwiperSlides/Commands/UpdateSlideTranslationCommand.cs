using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.SwiperSlides.Commands;

public record UpdateSlideTranslationCommand(
    Guid SlideId,
    string LanguageCode,
    string TopText,
    string BigTitle,
    string HighlightedTitleNormal,
    string HighlightedTitleColor,
    string? HighlightedTitleBold,
    string BottomText
) : IRequest<Result>;

public class UpdateSlideTranslationCommandValidator : AbstractValidator<UpdateSlideTranslationCommand>
{
    public UpdateSlideTranslationCommandValidator()
    {
        RuleFor(x => x.SlideId).NotEmpty();
        RuleFor(x => x.LanguageCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.TopText).NotEmpty().MaximumLength(300);
        RuleFor(x => x.BigTitle).NotEmpty().MaximumLength(500);
        RuleFor(x => x.HighlightedTitleNormal).NotEmpty().MaximumLength(300);
        RuleFor(x => x.HighlightedTitleColor).NotEmpty().MaximumLength(300);
        RuleFor(x => x.BottomText).NotEmpty().MaximumLength(500);
    }
}

public class UpdateSlideTranslationCommandHandler : IRequestHandler<UpdateSlideTranslationCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public UpdateSlideTranslationCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result> Handle(UpdateSlideTranslationCommand request, CancellationToken cancellationToken)
    {
        var slide = await _context.SwiperSlides.FindAsync([request.SlideId], cancellationToken);
        if (slide is null) return Result.NotFound("Slide not found.");

        var translation = await _context.SwiperSlideTranslations
            .FirstOrDefaultAsync(t => t.SwiperSlideId == request.SlideId && t.LanguageCode == request.LanguageCode, cancellationToken);

        if (translation is null)
        {
            _context.SwiperSlideTranslations.Add(new SwiperSlideTranslation
            {
                SwiperSlideId = request.SlideId,
                LanguageCode = request.LanguageCode,
                TopText = request.TopText,
                BigTitle = request.BigTitle,
                HighlightedTitleNormal = request.HighlightedTitleNormal,
                HighlightedTitleColor = request.HighlightedTitleColor,
                HighlightedTitleBold = request.HighlightedTitleBold,
                BottomText = request.BottomText
            });
        }
        else
        {
            translation.TopText = request.TopText;
            translation.BigTitle = request.BigTitle;
            translation.HighlightedTitleNormal = request.HighlightedTitleNormal;
            translation.HighlightedTitleColor = request.HighlightedTitleColor;
            translation.HighlightedTitleBold = request.HighlightedTitleBold;
            translation.BottomText = request.BottomText;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("home-slides:", cancellationToken);
        return Result.Success();
    }
}
