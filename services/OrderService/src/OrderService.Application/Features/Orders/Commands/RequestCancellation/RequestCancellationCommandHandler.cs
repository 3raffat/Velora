using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Exceptions;
using OrderService.Domain.Common.Exceptions;
using OrderService.Domain.Common.ValueObjects;
using OrderService.Domain.Entities.Orders;
using OrderService.Domain.Entities.Orders.Enums;

namespace OrderService.Application.Features.Orders.Commands.RequestCancellation;

public sealed class RequestCancellationCommandHandler(IVeloraContext _context)
    : IRequestHandler<RequestCancellationCommand>
{
    public async Task Handle(RequestCancellationCommand request, CancellationToken ct)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(
            o => o.Id == request.OrderId && o.CustomerId == request.CustomerId,
            ct
        );

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        if (order.OrderStatus != OrderStatus.Confirmed)
            throw new InvalidStatusException(
                nameof(Order),
                nameof(Order.Cancel),
                order.OrderStatus,
                OrderStatus.Confirmed
            );

        var hasPendingCancellation = await _context.Cancellations.AnyAsync(
            c => c.OrderId == request.OrderId && c.Status == CancellationStatus.Pending,
            ct
        );

        if (hasPendingCancellation)
            throw new CancellationAlreadyPendingException(request.OrderId);

        var cancellation = Cancellation.Create(
            request.Reason,
            Money.Create(order.TotalAmount),
            order.Id
        );

        _context.Cancellations.Add(cancellation);

        await _context.SaveChangesAsync(ct);
    }
}
