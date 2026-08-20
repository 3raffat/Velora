using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Dtos;
using OrderService.Application.Features.Orders.Mapper;

namespace OrderService.Application.Features.Orders.Queries.GetCustomerOrders;

public sealed class GetCustomerOrdersQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetCustomerOrdersQuery, IReadOnlyCollection<OrderSummaryDto>>
{
    public async Task<IReadOnlyCollection<OrderSummaryDto>> Handle(
        GetCustomerOrdersQuery request,
        CancellationToken ct
    )
    {
        var orders = await _context
            .Orders.Include(o => o.OrderItems)
            .Where(o => o.CustomerId == request.CustomerId)
            .OrderByDescending(o => o.OrderDate)
            .AsNoTracking()
            .ToListAsync(ct);

        return orders.Select(o => o.ToSummaryDto()).ToList();
    }
}
