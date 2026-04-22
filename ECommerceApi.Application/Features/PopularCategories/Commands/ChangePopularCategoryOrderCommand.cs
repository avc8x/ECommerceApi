using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Application.Features.PopularCategories.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.PopularCategories.Commands;

public record ChangePopularCategoryOrderCommand(IEnumerable<NewItemOrderDto> Orders) : IRequest<Result>;

public class ChangePopularCategoryOrderCommandHandler : IRequestHandler<ChangePopularCategoryOrderCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public ChangePopularCategoryOrderCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result> Handle(ChangePopularCategoryOrderCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Orders.Select(o => o.Id).ToList();
        var entries = await _context.PopularCategories
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(cancellationToken);

        foreach (var entry in entries)
        {
            var order = request.Orders.FirstOrDefault(o => o.Id == entry.Id);
            if (order is not null) entry.DisplayOrder = order.NewOrder;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("popular-categories:", cancellationToken);
        return Result.Success();
    }
}
