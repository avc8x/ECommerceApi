namespace ECommerceApi.Application.Features.SwiperSlides.DTOs;

public record SwiperSlideTranslationDto(
    string LanguageCode,
    string TopText,
    string BigTitle,
    string HighlightedTitleNormal,
    string HighlightedTitleColor,
    string? HighlightedTitleBold,
    string BottomText
);

public record SwiperSlideDto(
    Guid Id,
    string ImageUrl,
    int DisplayOrder,
    Guid CategoryId,
    string CategorySlug,
    IEnumerable<SwiperSlideTranslationDto> Translations
);

public record SwiperSlideClientDto(
    Guid Id,
    string ImageUrl,
    int DisplayOrder,
    Guid CategoryId,
    string CategorySlug,
    string TopText,
    string BigTitle,
    string HighlightedTitleNormal,
    string HighlightedTitleColor,
    string? HighlightedTitleBold,
    string BottomText
);

public record NewItemOrderDto(Guid Id, int NewOrder);
