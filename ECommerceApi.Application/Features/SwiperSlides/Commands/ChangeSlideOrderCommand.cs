using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Application.Common.Results;
using ECommerceApi.Application.Features.SwiperSlides.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Application.Features.SwiperSlides.Commands;

public record ChangeSlideOrderCommand(IEnumerable<NewItemOrderDto> Orders) : IRequest<Result>;

public class ChangeSlideOrderCommandHandler : IRequestHandler<ChangeSlideOrderCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public ChangeSlideOrderCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result> Handle(ChangeSlideOrderCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Orders.Select(o => o.Id).ToList();
        var slides = await _context.SwiperSlides
            .Where(s => ids.Contains(s.Id))
            .ToListAsync(cancellationToken);

        foreach (var slide in slides)
        {
            var order = request.Orders.FirstOrDefault(o => o.Id == slide.Id);
            if (order is not null) slide.DisplayOrder = order.NewOrder;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByPrefixAsync("home-slides:", cancellationToken);
        return Result.Success();
    }
}
