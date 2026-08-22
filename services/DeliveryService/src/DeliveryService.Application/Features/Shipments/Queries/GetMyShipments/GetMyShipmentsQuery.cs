using DeliveryService.Application.Features.Shipments.Dtos;
using MediatR;

namespace DeliveryService.Application.Features.Shipments.Queries.GetMyShipments;

public sealed record GetMyShipmentsQuery(Guid DriverId)
    : IRequest<IReadOnlyCollection<ShipmentDto>>;
