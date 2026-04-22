using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CategoryTranslation> CategoryTranslations => Set<CategoryTranslation>();
    public DbSet<PopularCategory> PopularCategories => Set<PopularCategory>();
    public DbSet<SwiperSlide> SwiperSlides => Set<SwiperSlide>();
    public DbSet<SwiperSlideTranslation> SwiperSlideTranslations => Set<SwiperSlideTranslation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Auto-set UpdatedAt on modified entities
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Modified && entry.Entity is Domain.Common.BaseEntity entity)
                entity.UpdatedAt = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
