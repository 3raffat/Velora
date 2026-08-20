using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Dtos;
using OrderService.Application.Features.Orders.Exceptions;
using OrderService.Application.Features.Orders.Mapper;

namespace OrderService.Application.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetOrderByIdQuery, OrderDetailDto>
{
    public async Task<OrderDetailDto> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        var order = await _context
            .Orders.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Payment)
            .Include(o => o.Cancellation)
                .ThenInclude(c => c!.Refund)
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
