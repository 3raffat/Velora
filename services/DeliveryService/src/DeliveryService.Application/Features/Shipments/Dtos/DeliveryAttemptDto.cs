namespace DeliveryService.Application.Features.Shipments.Dtos;

public sealed record DeliveryAttemptDto(
    Guid Id,
    Guid ShipmentId,
    DateTime AttemptedAt,
    string FailureReason
);
