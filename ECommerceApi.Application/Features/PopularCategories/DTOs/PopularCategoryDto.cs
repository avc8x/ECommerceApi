namespace ECommerceApi.Application.Features.PopularCategories.DTOs;

public record PopularCategoryDto(
    Guid Id,
    Guid CategoryId,
    string CategorySlug,
    string Title,
    string? ImageUrl,
    int DisplayOrder
);

public record NewItemOrderDto(Guid Id, int NewOrder);
