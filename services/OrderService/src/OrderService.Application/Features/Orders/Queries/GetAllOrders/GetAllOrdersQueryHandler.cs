using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Dtos;
using OrderService.Application.Features.Orders.Mapper;

namespace OrderService.Application.Features.Orders.Queries.GetAllOrders;

public sealed class GetAllOrdersQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetAllOrdersQuery, IReadOnlyCollection<OrderSummaryDto>>
{
    public async Task<IReadOnlyCollection<OrderSummaryDto>> Handle(
        GetAllOrdersQuery request,
        CancellationToken ct
    )
    {
        var orders = await _context
            .Orders.Include(o => o.OrderItems)
            .OrderByDescending(o => o.OrderDate)
            .AsNoTracking()
            .ToListAsync(ct);

        return orders.Select(o => o.ToSummaryDto()).ToList();
    }
}
