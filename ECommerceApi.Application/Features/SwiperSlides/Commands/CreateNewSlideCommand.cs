using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.SwiperSlides.Commands;

public record CreateNewSlideCommand(
    string ImageUrl,
    Guid CategoryId,
    string LanguageCode,
    string TopText,
    string BigTitle,
    string HighlightedTitleNormal,
    string HighlightedTitleColor,
    string? HighlightedTitleBold,
    string BottomText
) : IRequest<Result<Guid>>;

public class CreateNewSlideCommandValidator : AbstractValidator<CreateNewSlideCommand>
{
    public CreateNewSlideCommandValidator()
    {
        RuleFor(x => x.ImageUrl).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.LanguageCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.TopText).NotEmpty().MaximumLength(300);
        RuleFor(x => x.BigTitle).NotEmpty().MaximumLength(500);
        RuleFor(x => x.HighlightedTitleNormal).NotEmpty().MaximumLength(300);
        RuleFor(x => x.HighlightedTitleColor).NotEmpty().MaximumLength(300);
        RuleFor(x => x.BottomText).NotEmpty().MaximumLength(500);
    }
}

public class CreateNewSlideCommandHandler : IRequestHandler<CreateNewSlideCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public CreateNewSlideCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<Guid>> Handle(CreateNewSlideCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync([request.CategoryId], cancellationToken);
        if (category is null) return Result<Guid>.NotFound("Category not found.");

        var maxOrder = await _context.SwiperSlides
            .MaxAsync(s => (int?)s.DisplayOrder, cancellationToken) ?? -1;

        var slide = new SwiperSlide
        {
            ImageUrl = request.ImageUrl,
            CategoryId = request.CategoryId,
            DisplayOrder = maxOrder + 1,
            Translations = new List<SwiperSlideTranslation>
            {
                new()
                {
                    LanguageCode = request.LanguageCode,
                    TopText = request.TopText,
                    BigTitle = request.BigTitle,
                    HighlightedTitleNormal = request.HighlightedTitleNormal,
                    HighlightedTitleColor = request.HighlightedTitleColor,
                    HighlightedTitleBold = request.HighlightedTitleBold,
                    BottomText = request.BottomText
                }
            }
        };

        _context.SwiperSlides.Add(slide);
        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("home-slides:", cancellationToken);

        return Result<Guid>.Created(slide.Id);
    }
}
