using MediatR;
using Microsoft.EntityFrameworkCore;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Orders.Dtos;
using Velora.Application.Features.Orders.Mapper;

namespace Velora.Application.Features.Orders.Queries.GetCustomerOrders;

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
