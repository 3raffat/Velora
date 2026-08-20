using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Exceptions;
using OrderService.Domain.Entities.Orders.Events;

namespace OrderService.Application.Features.Orders.Commands.ApproveCancellation;

public sealed class ApproveCancellationCommandHandler(
    IVeloraContext _context,
    ILogger<ApproveCancellationCommandHandler> _logger
) : IRequestHandler<ApproveCancellationCommand>
{
    public async Task Handle(ApproveCancellationCommand request, CancellationToken ct)
    {
        var cancellation = await _context
            .Cancellations.Include(c => c.Order)
                .ThenInclude(o => o.Payment)
            .FirstOrDefaultAsync(c => c.OrderId == request.OrderId, ct);

        if (cancellation is null)
            throw new CancellationNotFoundException(request.OrderId);

        cancellation.Approve(request.ProcessedBy, request.CancellationCharges);

        cancellation.Order.Cancel();

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Cancellation {CancellationId} approved for order {OrderId} by {ProcessedBy}",
            cancellation.Id,
            request.OrderId,
            request.ProcessedBy
        );
    }
}
