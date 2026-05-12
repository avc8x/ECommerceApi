namespace ECommerceApi.Application.Features.Categories.DTOs;

public record CategoryTranslationDto(
    string LanguageCode,
    string Title,
    string? Description,
    string? MetaTitle,
    string? MetaDescription
);

public record CategoryDto(
    Guid Id,
    string Slug,
    string? ImageUrl,
    int SortOrder,
    Guid? ParentId,
    IEnumerable<CategoryTranslationDto> Translations,
    IEnumerable<CategoryDto> Children
);

/// <summary>Client-facing DTO: returns only the requested language's translation.</summary>
public record CategoryTreeDto(
    Guid Id,
    string Slug,
    string Title,
    string? Description,
    string? ImageUrl,
    Guid? ParentId,
    IEnumerable<CategoryTreeDto> Children
);
