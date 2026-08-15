using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Orders.Exceptions;

namespace Velora.Application.Features.Orders.Commands.RejectCancellation;

public sealed class RejectCancellationCommandHandler(
    IVeloraContext _context,
    ILogger<RejectCancellationCommandHandler> _logger
) : IRequestHandler<RejectCancellationCommand>
{
    public async Task Handle(RejectCancellationCommand request, CancellationToken ct)
    {
        var cancellation = await _context.Cancellations.FirstOrDefaultAsync(
            c => c.OrderId == request.OrderId,
            ct
        );

        if (cancellation is null)
            throw new CancellationNotFoundException(request.OrderId);

        cancellation.Reject(request.ProcessedBy, request.Remarks);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Cancellation {CancellationId} rejected for order {OrderId} by {ProcessedBy}. Remarks: {Remarks}",
            cancellation.Id,
            request.OrderId,
            request.ProcessedBy,
            request.Remarks
        );
    }
}
