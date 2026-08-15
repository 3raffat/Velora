using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Orders.Exceptions;

namespace Velora.Application.Features.Orders.Commands.ApproveRefund;

public sealed class ApproveRefundCommandHandler(
    IVeloraContext _context,
    ILogger<ApproveRefundCommandHandler> _logger
) : IRequestHandler<ApproveRefundCommand>
{
    public async Task Handle(ApproveRefundCommand request, CancellationToken ct)
    {
        var refund = await _context
            .Refunds.Include(r => r.Cancellation)
            .FirstOrDefaultAsync(r => r.Cancellation.OrderId == request.OrderId, ct);

        if (refund is null)
            throw new RefundNotFoundException(request.OrderId);

        refund.Approve();

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Refund {RefundId} approved for order {OrderId} by {ProcessedBy}",
            refund.Id,
            request.OrderId,
            request.ProcessedBy
        );
    }
}
