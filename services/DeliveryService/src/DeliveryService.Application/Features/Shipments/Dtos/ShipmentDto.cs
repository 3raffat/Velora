namespace DeliveryService.Application.Features.Shipments.Dtos;

public sealed record ShipmentDto(
    Guid Id,
    Guid OrderId,
    string TrackingNumber,
    string RecipientName,
    string RecipientPhone,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string Country,
    decimal TotalAmount,
    string Status,
    Guid? DriverId,
    DateTime? PickedUpAt,
    DateTime? DeliveredAt,
    string? FailureReason,
    IReadOnlyCollection<DeliveryAttemptDto> DeliveryAttempts
);