using MediatR;
using Microsoft.EntityFrameworkCore;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Orders.Dtos;
using Velora.Application.Features.Orders.Exceptions;
using Velora.Application.Features.Orders.Mapper;

namespace Velora.Application.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetOrderByIdQuery, OrderDetailDto>
{
    public async Task<OrderDetailDto> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        var order = await _context
            .Orders.Include(o => o.OrderItems)
            .Include(o => o.Payment)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                o => o.Id == request.OrderId && o.CustomerId == request.CustomerId,
                ct
            );

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        return order.ToDetailDto();
    }
}
