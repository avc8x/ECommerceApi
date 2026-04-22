using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using FluentValidation;
using MediatR;

namespace ECommerceApi.Application.Features.SwiperSlides.Commands;

public record UpdateSlideCommand(Guid Id, string ImageUrl, Guid CategoryId) : IRequest<Result>;

public class UpdateSlideCommandValidator : AbstractValidator<UpdateSlideCommand>
{
    public UpdateSlideCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ImageUrl).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}

public class UpdateSlideCommandHandler : IRequestHandler<UpdateSlideCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public UpdateSlideCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result> Handle(UpdateSlideCommand request, CancellationToken cancellationToken)
    {
        var slide = await _context.SwiperSlides.FindAsync([request.Id], cancellationToken);
        if (slide is null) return Result.NotFound("Slide not found.");

        var category = await _context.Categories.FindAsync([request.CategoryId], cancellationToken);
        if (category is null) return Result.NotFound("Category not found.");

        slide.ImageUrl = request.ImageUrl;
        slide.CategoryId = request.CategoryId;
        slide.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("home-slides:", cancellationToken);
        return Result.Success();
    }
}
