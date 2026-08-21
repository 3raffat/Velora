using DeliveryService.Application.Common.Exceptions;
using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Features.DeliveryAttempts.Mappers;
using DeliveryService.Application.Features.Shipments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Application.Features.DeliveryAttempts.Queries.GetDeliveryAttempts;

public sealed record GetDeliveryAttemptsQuery(Guid ShipmentId)
    : IRequest<IReadOnlyCollection<DeliveryAttemptDto>>;

public sealed class GetDeliveryAttemptsQueryHandler(IDeliveryContext context)
    : IRequestHandler<GetDeliveryAttemptsQuery, IReadOnlyCollection<DeliveryAttemptDto>>
{
    public async Task<IReadOnlyCollection<DeliveryAttemptDto>> Handle(
        GetDeliveryAttemptsQuery request,
        CancellationToken ct
    )
    {
        var shipmentExists = await context.Shipments.AnyAsync(
            shipment => shipment.Id == request.ShipmentId,
            ct
        );

        if (!shipmentExists)
            throw new NotFoundException("Shipment was not found.");

        var attempts = await context
            .DeliveryAttempts.AsNoTracking()
            .Where(attempt => attempt.ShipmentId == request.ShipmentId)
            .OrderByDescending(attempt => attempt.AttemptedAt)
            .ToListAsync(ct);

        return attempts.ToDto();
    }
}
