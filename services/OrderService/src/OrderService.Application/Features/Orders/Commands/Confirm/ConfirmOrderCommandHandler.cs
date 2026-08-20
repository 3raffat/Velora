using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Exceptions;

namespace OrderService.Application.Features.Orders.Commands.Confirm;

public sealed class ConfirmOrderCommandHandler(
    IVeloraContext _context,
    ILogger<ConfirmOrderCommandHandler> _logger
) : IRequestHandler<ConfirmOrderCommand>
{
    public async Task Handle(ConfirmOrderCommand request, CancellationToken ct)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(
            o => o.Id == request.OrderId && o.CustomerId == request.CustomerId,
            ct
        );

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        order.Confirm();

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Order {OrderId} confirmed for customer {CustomerId}",
            order.Id,
            request.CustomerId
        );
    }
}
