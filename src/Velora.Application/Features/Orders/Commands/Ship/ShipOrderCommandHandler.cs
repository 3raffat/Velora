using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Orders.Exceptions;

namespace Velora.Application.Features.Orders.Commands.Ship;

public sealed class ShipOrderCommandHandler(
    IVeloraContext _context,
    ILogger<ShipOrderCommandHandler> _logger
) : IRequestHandler<ShipOrderCommand>
{
    public async Task Handle(ShipOrderCommand request, CancellationToken ct)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(
            o => o.Id == request.OrderId && o.CustomerId == request.CustomerId,
            ct
        );

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        order.Ship();

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Order {OrderId} shipped for customer {CustomerId}",
            order.Id,
            request.CustomerId
        );
    }
}
