using ECommerceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<CategoryTranslation> CategoryTranslations { get; }
    DbSet<PopularCategory> PopularCategories { get; }
    DbSet<SwiperSlide> SwiperSlides { get; }
    DbSet<SwiperSlideTranslation> SwiperSlideTranslations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
