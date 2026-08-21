using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Features.Shipments.Dtos;
using MediatR;

namespace DeliveryService.Application.Features.Shipments.Commands.AssignShipmentDriver;

public sealed record AssignShipmentDriverCommand(Guid ShipmentId, Guid DriverId)
    : IRequest<ShipmentDto>;
