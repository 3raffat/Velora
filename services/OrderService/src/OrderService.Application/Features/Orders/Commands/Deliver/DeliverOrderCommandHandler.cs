using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Exceptions;

namespace OrderService.Application.Features.Orders.Commands.Deliver;

public sealed class DeliverOrderCommandHandler(
    IVeloraContext _context,
    ILogger<DeliverOrderCommandHandler> _logger
) : IRequestHandler<DeliverOrderCommand>
{
    public async Task Handle(DeliverOrderCommand request, CancellationToken ct)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(
            o => o.Id == request.OrderId && o.CustomerId == request.CustomerId,
            ct
        );

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        order.Deliver();

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Order {OrderId} delivered for customer {CustomerId}",
            order.Id,
            request.CustomerId
        );
    }
}
