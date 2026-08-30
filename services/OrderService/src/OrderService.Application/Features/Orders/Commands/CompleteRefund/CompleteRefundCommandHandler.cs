using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Exceptions;
using OrderService.Domain.Entities.Orders.Enums;

namespace OrderService.Application.Features.Orders.Commands.CompleteRefund;

public sealed class CompleteRefundCommandHandler(
    IVeloraContext _context,
    IPayPalClient _paypalClient,
    ILogger<CompleteRefundCommandHandler> _logger
) : IRequestHandler<CompleteRefundCommand>
{
    public async Task Handle(CompleteRefundCommand request, CancellationToken ct)
    {
        var refund = await _context
            .Refunds.Include(r => r.Payment)
            .Include(r => r.Cancellation)
            .FirstOrDefaultAsync(r => r.Cancellation.OrderId == request.OrderId, ct);

        if (refund is null)
            throw new RefundNotFoundException(request.OrderId);

        string transactionId;

        if (refund.Payment.PaymentMethod == PaymentMethod.PayPal)
        {
            if (string.IsNullOrWhiteSpace(refund.Payment.TransactionId))
            {
                throw new InvalidOperationException(
                    "Cannot process PayPal refund: original payment has no transaction/capture ID."
                );
            }

            transactionId = await _paypalClient.RefundCaptureAsync(
                refund.Payment.TransactionId,
                refund.Amount.Amount,
                "USD",
                ct
            );
        }
        else
        {
            if (string.IsNullOrWhiteSpace(request.TransactionId))
            {
                throw new InvalidOperationException(
                    "TransactionId is required to complete non-PayPal refunds."
                );
            }

            transactionId = request.TransactionId;
        }

        refund.Complete(request.ProcessedBy, transactionId);

        refund.Payment.MarkRefunded();

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Refund {RefundId} completed for order {OrderId}. TransactionId: {TransactionId}",
            refund.Id,
            request.OrderId,
            transactionId
        );
    }
}
