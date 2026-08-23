using DeliveryService.Domain.Common;
using DeliveryService.Domain.Common.Exceptions;

namespace DeliveryService.Domain.Entities.Shipments;

public sealed class DeliveryAttempt : BaseEntity
{
    public Guid ShipmentId { get; private set; }
    public Guid DriverId { get; private set; }
    public DateTime AttemptedAt { get; private set; }
    public string FailureReason { get; private set; } = string.Empty;

    private DeliveryAttempt() { }

    private DeliveryAttempt(Guid id, Guid shipmentId, Guid driverId, string failureReason)
        : base(id)
    {
        ShipmentId = shipmentId;
        DriverId = driverId;
        AttemptedAt = DateTime.UtcNow;
        FailureReason = failureReason;
    }

    public static DeliveryAttempt Create(Guid shipmentId, Guid driverId, string failureReason)
    {
        if (shipmentId == Guid.Empty)
            throw new RequiredFieldException(nameof(shipmentId));

        if (driverId == Guid.Empty)
            throw new RequiredFieldException(nameof(driverId));

        if (string.IsNullOrWhiteSpace(failureReason))
            throw new RequiredFieldException(nameof(failureReason));

        return new DeliveryAttempt(Guid.NewGuid(), shipmentId, driverId, failureReason.Trim());
    }
}
