using DeliveryService.Domain.Common;
using DeliveryService.Domain.Common.Exceptions;

namespace DeliveryService.Domain.Entities.Shipments;

public sealed class DeliveryAttempt : BaseEntity
{
    public Guid ShipmentId { get; private set; }
    public DateTime AttemptedAt { get; private set; }
    public string FailureReason { get; private set; } = string.Empty;

    private DeliveryAttempt() { }

    private DeliveryAttempt(Guid id, Guid shipmentId, string failureReason)
        : base(id)
    {
        ShipmentId = shipmentId;
        AttemptedAt = DateTime.UtcNow;
        FailureReason = failureReason;
    }

    public static DeliveryAttempt Create(Guid shipmentId, string failureReason)
    {
        if (shipmentId == Guid.Empty)
            throw new RequiredFieldException(nameof(shipmentId));

        if (string.IsNullOrWhiteSpace(failureReason))
            throw new RequiredFieldException(nameof(failureReason));

        return new DeliveryAttempt(Guid.NewGuid(), shipmentId, failureReason.Trim());
    }
}
