using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Domain.Entities.Shipments;

namespace DeliveryService.Application.Features.DeliveryAttempts.Mappers;

public static class DeliveryAttemptMapper
{
    public static DeliveryAttemptDto ToDto(this DeliveryAttempt attempt) =>
        new(attempt.Id, attempt.ShipmentId, attempt.AttemptedAt, attempt.FailureReason);

    public static IReadOnlyCollection<DeliveryAttemptDto> ToDto(
        this IEnumerable<DeliveryAttempt> attempts
    ) => attempts.Select(attempt => attempt.ToDto()).ToArray();
}
