using DeliveryService.Application.Features.Shipments.Dtos;
using MediatR;

namespace DeliveryService.Application.Features.Shipments.Queries.GetShipmentsByOrderId;

public sealed record GetShipmentsByOrderIdQuery(Guid OrderId) : IRequest<ShipmentTrackingDto>;
