using DeliveryService.Domain.Entities.Shipments.Enums;

namespace DeliveryService.Application.Features.Shipments.Dtos;

public sealed record ShipmentTrackingDto(
    Guid OrderId,
    string TrackingNumber,
    ShipmentStatus Status,
    DateTime? PickedUpAt,
    DateTime? DeliveredAt,
    string? FailureReason
);
