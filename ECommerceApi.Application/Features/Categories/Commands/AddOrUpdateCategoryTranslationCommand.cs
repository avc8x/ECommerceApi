using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.Categories.Commands;

public record AddOrUpdateCategoryTranslationCommand(
    Guid CategoryId,
    string LanguageCode,
    string Title,
    string? Description,
    string? MetaTitle,
    string? MetaDescription
) : IRequest<Result>;

public class AddOrUpdateCategoryTranslationCommandValidator : AbstractValidator<AddOrUpdateCategoryTranslationCommand>
{
    public AddOrUpdateCategoryTranslationCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.LanguageCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}

public class AddOrUpdateCategoryTranslationCommandHandler : IRequestHandler<AddOrUpdateCategoryTranslationCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public AddOrUpdateCategoryTranslationCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result> Handle(AddOrUpdateCategoryTranslationCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync([request.CategoryId], cancellationToken);
        if (category is null) return Result.NotFound("Category not found.");

        var existing = await _context.CategoryTranslations
            .FirstOrDefaultAsync(t => t.CategoryId == request.CategoryId && t.LanguageCode == request.LanguageCode, cancellationToken);

        if (existing is null)
        {
            _context.CategoryTranslations.Add(new CategoryTranslation
            {
                CategoryId = request.CategoryId,
                LanguageCode = request.LanguageCode,
                Title = request.Title,
                Description = request.Description,
                MetaTitle = request.MetaTitle,
                MetaDescription = request.MetaDescription
            });
        }
        else
        {
            existing.Title = request.Title;
            existing.Description = request.Description;
            existing.MetaTitle = request.MetaTitle;
            existing.MetaDescription = request.MetaDescription;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("categories:", cancellationToken);
        return Result.Success();
    }
}
