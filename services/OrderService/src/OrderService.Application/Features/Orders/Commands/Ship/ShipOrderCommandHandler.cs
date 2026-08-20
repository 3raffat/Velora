using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Exceptions;

namespace OrderService.Application.Features.Orders.Commands.Ship;

public sealed class ShipOrderCommandHandler(
    IVeloraContext _context,
    IDeliveryClient _deliveryClient,
    ILogger<ShipOrderCommandHandler> _logger
) : IRequestHandler<ShipOrderCommand>
{
    public async Task Handle(ShipOrderCommand request, CancellationToken ct)
    {
        var order = await _context
            .Orders.Include(o => o.Customer)
            .FirstOrDefaultAsync(
                o => o.Id == request.OrderId && o.CustomerId == request.CustomerId,
                ct
            );

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        var shipment = await _deliveryClient.CreateShipmentAsync(
            new Common.Models.CreateShipmentRequest(
                order.Id,
                order.Customer.FirstName.Value,
                order.Customer.PhoneNumber.Value,
                order.ShippingAddress,
                order.TotalAmount
            ),
            ct
        );

        order.Ship();

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Order {OrderId} shipped for customer {CustomerId}",
            order.Id,
            request.CustomerId
        );
    }
}
