using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Exceptions;

namespace OrderService.Application.Features.Orders.Commands.CompleteRefund;

public sealed class CompleteRefundCommandHandler(
    IVeloraContext _context,
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

        refund.Complete(request.ProcessedBy, request.TransactionId);

        refund.Payment.MarkRefunded();

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Refund {RefundId} completed for order {OrderId}. TransactionId: {TransactionId}",
            refund.Id,
            request.OrderId,
            request.TransactionId
        );
    }
}
