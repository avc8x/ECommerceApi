using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using FluentValidation;
using MediatR;

namespace ECommerceApi.Application.Features.PopularCategories.Commands;

public record RemoveCategoryFromPopularCategoriesCommand(Guid Id) : IRequest<Result>;

public class RemoveCategoryFromPopularCategoriesCommandValidator : AbstractValidator<RemoveCategoryFromPopularCategoriesCommand>
{
    public RemoveCategoryFromPopularCategoriesCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class RemoveCategoryFromPopularCategoriesCommandHandler : IRequestHandler<RemoveCategoryFromPopularCategoriesCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public RemoveCategoryFromPopularCategoriesCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result> Handle(RemoveCategoryFromPopularCategoriesCommand request, CancellationToken cancellationToken)
    {
        var popular = await _context.PopularCategories.FindAsync([request.Id], cancellationToken);
        if (popular is null) return Result.NotFound("Popular category entry not found.");

        _context.PopularCategories.Remove(popular);
        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("popular-categories:", cancellationToken);
        return Result.Success();
    }
}
