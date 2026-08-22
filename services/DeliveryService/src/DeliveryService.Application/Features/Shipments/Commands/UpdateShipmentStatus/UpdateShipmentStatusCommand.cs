using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Domain.Entities.Shipments.Enums;
using MediatR;

namespace DeliveryService.Application.Features.Shipments.Commands.UpdateShipmentStatus;

public sealed record UpdateShipmentStatusCommand(
    Guid ShipmentId,
    Guid DriverId,
    ShipmentStatus Status,
    string? FailureReason = null
) : IRequest<ShipmentDto>;
