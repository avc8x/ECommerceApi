using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.PopularCategories.Commands;

public record AddCategoryToPopularCategoriesCommand(Guid CategoryId) : IRequest<Result<Guid>>;

public class AddCategoryToPopularCategoriesCommandValidator : AbstractValidator<AddCategoryToPopularCategoriesCommand>
{
    public AddCategoryToPopularCategoriesCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}

public class AddCategoryToPopularCategoriesCommandHandler : IRequestHandler<AddCategoryToPopularCategoriesCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public AddCategoryToPopularCategoriesCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<Guid>> Handle(AddCategoryToPopularCategoriesCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync([request.CategoryId], cancellationToken);
        if (category is null) return Result<Guid>.NotFound("Category not found.");

        // Must be a root (level 1) category — no parent
        if (category.ParentId.HasValue)
            return Result<Guid>.Failure("Only root-level (level 1) categories can be added as popular.");

        var alreadyExists = await _context.PopularCategories
            .AnyAsync(p => p.CategoryId == request.CategoryId, cancellationToken);
        if (alreadyExists)
            return Result<Guid>.Conflict("Category is already in popular categories.");

        var maxOrder = await _context.PopularCategories
            .MaxAsync(p => (int?)p.DisplayOrder, cancellationToken) ?? -1;

        var popular = new PopularCategory
        {
            CategoryId = request.CategoryId,
            DisplayOrder = maxOrder + 1
        };

        _context.PopularCategories.Add(popular);
        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("popular-categories:", cancellationToken);

        return Result<Guid>.Created(popular.Id);
    }
}
