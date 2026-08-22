using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Application.Features.Shipments.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Application.Features.Shipments.Queries.GetShipments;

public sealed class GetShipmentsQueryHandler(IDeliveryContext context)
    : IRequestHandler<GetShipmentsQuery, IReadOnlyCollection<ShipmentDto>>
{
    public async Task<IReadOnlyCollection<ShipmentDto>> Handle(
        GetShipmentsQuery request,
        CancellationToken ct
    )
    {
        var query = context.Shipments.AsNoTracking().AsQueryable();

        if (request.OrderId.HasValue)
            query = query.Where(shipment => shipment.OrderId == request.OrderId.Value);

        if (!string.IsNullOrWhiteSpace(request.TrackingNumber))
            query = query.Where(shipment =>
                shipment.TrackingNumber.Value == request.TrackingNumber
            );

        if (request.DriverId.HasValue)
        {
            query = query.Where(shipment => shipment.DriverId == request.DriverId.Value);
        }

        var shipments = await query
            .OrderByDescending(shipment => shipment.CreatedAt)
            .ToListAsync(ct);

        return shipments.ToDto();
    }
}
