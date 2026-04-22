using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using FluentValidation;
using MediatR;

namespace ECommerceApi.Application.Features.Categories.Commands;

public record UpdateCategoryCommand(
    Guid Id,
    string? ImageUrl,
    Guid? ParentId
) : IRequest<Result>;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public UpdateCategoryCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync([request.Id], cancellationToken);
        if (category is null) return Result.NotFound("Category not found.");

        if (request.ParentId.HasValue && request.ParentId.Value == request.Id)
            return Result.Failure("A category cannot be its own parent.");

        if (request.ParentId.HasValue)
        {
            var parent = await _context.Categories.FindAsync([request.ParentId.Value], cancellationToken);
            if (parent is null) return Result.NotFound("Parent category not found.");
        }

        category.ImageUrl = request.ImageUrl;
        category.ParentId = request.ParentId;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("categories:", cancellationToken);
        return Result.Success();
    }
}
