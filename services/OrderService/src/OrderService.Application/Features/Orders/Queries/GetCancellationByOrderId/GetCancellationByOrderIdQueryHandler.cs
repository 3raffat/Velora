using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Dtos;
using OrderService.Application.Features.Orders.Exceptions;
using OrderService.Application.Features.Orders.Mapper;

namespace OrderService.Application.Features.Orders.Queries.GetCancellationByOrderId;

public sealed class GetCancellationByOrderIdQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetCancellationByOrderIdQuery, CancellationDto>
{
    public async Task<CancellationDto> Handle(
        GetCancellationByOrderIdQuery request,
        CancellationToken ct
    )
    {
        var cancellation = await _context
            .Cancellations.Include(c => c.Refund)
            .Include(c => c.Order)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.OrderId == request.OrderId && c.Order.CustomerId == request.CustomerId,
                ct
            );

        if (cancellation is null)
            throw new CancellationNotFoundException(request.OrderId);

        return cancellation.ToDto();
    }
}
