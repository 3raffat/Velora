using DeliveryService.Domain.Entities.Shipments.Enums;

namespace DeliveryService.Api.Contracts.Shipments;

public sealed record UpdateShipmentStatusRequest(
    ShipmentStatus Status,
    string? FailureReason = null
);
