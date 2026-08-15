using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Customers.Exceptions;
using Velora.Application.Features.Orders.Exceptions;
using Velora.Application.Features.Orders.Mapper;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Orders;
using Velora.Domain.Entities.Orders.Enums;
using Velora.Domain.Entities.Orders.Events;

namespace Velora.Application.Features.Orders.Events;

public sealed class OrderCancelledEventHandler(
    IVeloraContext _context,
    IEmailService _emailService,
    ILogger<OrderCancelledEventHandler> _logger
) : INotificationHandler<OrderCancelledEvent>
{
    public async Task Handle(OrderCancelledEvent notification, CancellationToken ct)
    {
        var cancellation = await _context
            .Cancellations.Include(c => c.Order)
                .ThenInclude(o => o.Payment)
            .Include(c => c.Order)
                .ThenInclude(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(c => c.Id == notification.CancellationId, ct);

        if (cancellation is null)
            throw new CancellationNotFoundException(notification.OrderId);

        var order = cancellation.Order;
        var payment = order.Payment;

        // Auto-create refund if the order had a completed payment
        if (payment is not null && payment.Status == PaymentStatus.Completed)
        {
            var refundAmount = order.TotalAmount - (cancellation.CancellationCharges ?? 0);

            var refund = Refund.Create(
                Money.Create(refundAmount),
                payment.PaymentMethod,
                cancellation.Reason,
                payment.Id,
                cancellation.Id
            );

            cancellation.AttachRefund(refund);
            await _context.Refunds.AddAsync(refund, ct);

            _logger.LogInformation(
                "Refund {RefundId} auto-created for cancelled order {OrderId}. Amount: {Amount}",
                refund.Id,
                order.Id,
                refundAmount
            );
        }

        // Send cancellation confirmation email
        var customer = await _context.Customers.FirstOrDefaultAsync(
            c => c.Id == notification.CustomerId,
            ct
        );

        if (customer is null)
            throw new CustomerNotFoundException(notification.CustomerId);

        var orderDetail = order.ToDetailDto();
        var cancellationDto = cancellation.ToDto();

        await _emailService.SendCancellationConfirmationEmailAsync(
            customer.Email!.Value,
            orderDetail,
            cancellationDto,
            ct
        );

        _logger.LogInformation(
            "Cancellation confirmation email sent for order {OrderId} to {Email}",
            order.Id,
            customer.Email.Value
        );
    }
}
