using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Orders.Exceptions;

namespace Velora.Application.Features.Orders.Commands.RejectRefund;

public sealed class RejectRefundCommandHandler(
    IVeloraContext _context,
    ILogger<RejectRefundCommandHandler> _logger
) : IRequestHandler<RejectRefundCommand>
{
    public async Task Handle(RejectRefundCommand request, CancellationToken ct)
    {
        var refund = await _context
            .Refunds.Include(r => r.Cancellation)
            .FirstOrDefaultAsync(r => r.Cancellation.OrderId == request.OrderId, ct);

        if (refund is null)
            throw new RefundNotFoundException(request.OrderId);

        refund.Reject(request.Reason);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Refund {RefundId} rejected for order {OrderId}. Reason: {Reason}",
            refund.Id,
            request.OrderId,
            request.Reason
        );
    }
}
