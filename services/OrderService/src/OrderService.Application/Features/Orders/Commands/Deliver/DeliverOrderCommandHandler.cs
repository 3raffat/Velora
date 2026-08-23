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
        var order = await _context
            .Orders.Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, ct);

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        order.Deliver();

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Order {OrderId} delivered for customer ", order.Id);
    }
}
