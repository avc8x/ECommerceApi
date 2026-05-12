using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using FluentValidation;
using MediatR;

namespace ECommerceApi.Application.Features.Categories.Commands;

public record SoftDeleteCategoryCommand(Guid Id) : IRequest<Result>;

public class SoftDeleteCategoryCommandValidator : AbstractValidator<SoftDeleteCategoryCommand>
{
    public SoftDeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class SoftDeleteCategoryCommandHandler : IRequestHandler<SoftDeleteCategoryCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public SoftDeleteCategoryCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result> Handle(SoftDeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync([request.Id], cancellationToken);
        if (category is null) return Result.NotFound("Category not found.");

        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("categories:", cancellationToken);
        return Result.Success();
    }
}
