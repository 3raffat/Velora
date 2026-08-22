using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Application.Features.Shipments.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Application.Features.Shipments.Queries.GetMyShipments;

public sealed class GetMyShipmentsQueryHandler(IDeliveryContext context)
    : IRequestHandler<GetMyShipmentsQuery, IReadOnlyCollection<ShipmentDto>>
{
    public async Task<IReadOnlyCollection<ShipmentDto>> Handle(
        GetMyShipmentsQuery request,
        CancellationToken ct
    )
    {
        var shipments = await context
            .Shipments.AsNoTracking()
            .Where(shipment => shipment.DriverId == request.DriverId)
            .OrderByDescending(shipment => shipment.CreatedAt)
            .ToListAsync(ct);

        return shipments.ToDto();
    }
}
