using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Domain.Entities;
using FluentValidation;
using MediatR;

namespace ECommerceApi.Application.Features.Categories.Commands;

// ── Command ──────────────────────────────────────────────────────────────────

public record CreateNewCategoryCommand(
    string Title,           // default language title (used to generate slug)
    string LanguageCode,    // default language code
    string? Description,
    string? ImageUrl,
    Guid? ParentId
) : IRequest<Result<Guid>>;

// ── Validator ─────────────────────────────────────────────────────────────────

public class CreateNewCategoryCommandValidator : AbstractValidator<CreateNewCategoryCommand>
{
    public CreateNewCategoryCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LanguageCode).NotEmpty().MaximumLength(10);
    }
}

// ── Handler ───────────────────────────────────────────────────────────────────

public class CreateNewCategoryCommandHandler : IRequestHandler<CreateNewCategoryCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISlugService _slugService;
    private readonly ICacheService _cache;

    public CreateNewCategoryCommandHandler(
        IApplicationDbContext context,
        ISlugService slugService,
        ICacheService cache)
    {
        _context = context;
        _slugService = slugService;
        _cache = cache;
    }

    public async Task<Result<Guid>> Handle(CreateNewCategoryCommand request, CancellationToken cancellationToken)
    {
        // Validate parent exists if provided
        if (request.ParentId.HasValue)
        {
            var parent = await _context.Categories.FindAsync([request.ParentId.Value], cancellationToken);
            if (parent is null)
                return Result<Guid>.NotFound("Parent category not found.");
        }

        var slug = await _slugService.EnsureUniqueSlugAsync(
            _slugService.GenerateSlug(request.Title),
            cancellationToken: cancellationToken);

        var category = new Category
        {
            Slug = slug,
            ImageUrl = request.ImageUrl,
            ParentId = request.ParentId,
            Translations = new List<CategoryTranslation>
            {
                new()
                {
                    LanguageCode = request.LanguageCode,
                    Title = request.Title,
                    Description = request.Description
                }
            }
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        // Invalidate category caches
        await _cache.RemoveByPrefixAsync("categories:", cancellationToken);

        return Result<Guid>.Created(category.Id);
    }
}
