using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Dtos;
using OrderService.Application.Features.Orders.Mapper;

namespace OrderService.Application.Features.Orders.Queries.GetAllCancellations;

public sealed class GetAllCancellationsQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetAllCancellationsQuery, IReadOnlyCollection<CancellationDto>>
{
    public async Task<IReadOnlyCollection<CancellationDto>> Handle(
        GetAllCancellationsQuery request,
        CancellationToken ct
    )
    {
        var cancellations = await _context
            .Cancellations.Include(c => c.Order)
            .Include(c => c.Refund)
            .OrderByDescending(c => c.RequestedAt)
            .AsNoTracking()
            .ToListAsync(ct);

        return cancellations.Select(c => c.ToDto()).ToList();
    }
}
