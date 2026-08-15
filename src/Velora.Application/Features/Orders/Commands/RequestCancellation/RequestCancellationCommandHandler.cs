using MediatR;
using Microsoft.EntityFrameworkCore;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Orders.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Orders;
using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Application.Features.Orders.Commands.RequestCancellation;

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
