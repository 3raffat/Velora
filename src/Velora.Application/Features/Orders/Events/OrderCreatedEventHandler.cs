using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Customers.Exceptions;
using Velora.Application.Features.Orders.Dtos;
using Velora.Application.Features.Orders.Exceptions;
using Velora.Application.Features.Orders.Mapper;
using Velora.Domain.Entities.Orders.Events;

namespace Velora.Application.Features.Orders.Events;

public sealed class OrderCreatedEventHandler(
    IVeloraContext _context,
    IEmailService _emailService,
    ILogger<OrderCreatedEventHandler> _logger
) : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken ct)
    {
        var order = await _context
            .Orders.Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
            .FirstOrDefaultAsync(
                o => o.CustomerId == notification.CustomerId && o.Id == notification.OrderId,
                ct
            );

        if (order is null)
            throw new OrderNotFoundException(notification.OrderId);

        var customer = await _context.Customers.FirstOrDefaultAsync(
            c => c.Id == notification.CustomerId,
            ct
        );

        if (customer is null)
            throw new CustomerNotFoundException(notification.CustomerId);

        var orderDetails = order.ToDetailDto();

        await _emailService.SendOrderConfirmationEmailAsync(
            customer.Email!.Value,
            orderDetails,
            ct
        );

        _logger.LogInformation(
            "Order confirmation email sent for order {OrderId} to {Email}",
            order.Id,
            customer.Email.Value
        );
    }
}
