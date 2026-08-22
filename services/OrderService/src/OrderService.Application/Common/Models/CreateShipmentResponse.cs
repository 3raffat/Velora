namespace OrderService.Application.Common.Models;

public sealed record CreateShipmentResponse(Guid OrderId, string TrackingNumber);

public sealed record ShipmentTrackingResponse(
    Guid OrderId,
    string TrackingNumber,
    string Status,
    DateTime? PickedUpAt,
    DateTime? DeliveredAt,
    string? FailureReason
);
