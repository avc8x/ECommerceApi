namespace ECommerceApi.Application.Common.Interfaces;

public interface ISlugService
{
    string GenerateSlug(string text);
    Task<string> EnsureUniqueSlugAsync(string baseSlug, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
