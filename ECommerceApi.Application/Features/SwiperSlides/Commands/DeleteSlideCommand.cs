using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using FluentValidation;
using MediatR;

namespace ECommerceApi.Application.Features.SwiperSlides.Commands;

public record DeleteSlideCommand(Guid Id) : IRequest<Result>;

public class DeleteSlideCommandValidator : AbstractValidator<DeleteSlideCommand>
{
    public DeleteSlideCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteSlideCommandHandler : IRequestHandler<DeleteSlideCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public DeleteSlideCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result> Handle(DeleteSlideCommand request, CancellationToken cancellationToken)
    {
        var slide = await _context.SwiperSlides.FindAsync([request.Id], cancellationToken);
        if (slide is null) return Result.NotFound("Slide not found.");

        slide.IsDeleted = true;
        slide.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("home-slides:", cancellationToken);
        return Result.Success();
    }
}
