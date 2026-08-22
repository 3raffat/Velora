using DeliveryService.Application.Common.Exceptions;
using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Application.Features.Shipments.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Application.Features.Shipments.Queries.GetShipmentsByOrderId;

public sealed class GetShipmentsByOrderIdQueryHandler(IDeliveryContext context)
    : IRequestHandler<GetShipmentsByOrderIdQuery, ShipmentTrackingDto>
{
    public async Task<ShipmentTrackingDto> Handle(
        GetShipmentsByOrderIdQuery request,
        CancellationToken ct
    )
    {
        var shipment = await context
            .Shipments.AsNoTracking()
            .Where(shipment => shipment.OrderId == request.OrderId)
            .OrderByDescending(shipment => shipment.CreatedAt)
            .FirstOrDefaultAsync(ct);

        if (shipment is null)
            throw new NotFoundException("Shipment was not found for the specified order.");

        return shipment.ToTrackingDto();
    }
}
