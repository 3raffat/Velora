using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Features.Shipments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Application.Features.Shipments.Queries.GetShipments;

public sealed record GetShipmentsQuery(
    Guid? OrderId = null,
    string? TrackingNumber = null,
    Guid? DriverId = null
) : IRequest<IReadOnlyCollection<ShipmentDto>>;
