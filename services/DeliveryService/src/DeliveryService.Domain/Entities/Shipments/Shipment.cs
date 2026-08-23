using DeliveryService.Domain.Common;
using DeliveryService.Domain.Common.Exceptions;
using DeliveryService.Domain.Common.ValueObjects;
using DeliveryService.Domain.Entities.Shipments.Enums;
using DeliveryService.Domain.Entities.Shipments.Events;
using DeliveryService.Domain.Entities.Shipments.Exceptions;
using DeliveryService.Domain.Entities.Shipments.ValueObjects;

namespace DeliveryService.Domain.Entities.Shipments;

public sealed class Shipment : AuditableEntity
{
    public Guid OrderId { get; private set; }
    public TrackingNumber TrackingNumber { get; private set; } = null!;
    public string RecipientName { get; private set; } = string.Empty;
    public string RecipientPhone { get; private set; } = string.Empty;
    public AddressSnapshot DeliveryAddress { get; private set; } = null!;
    public decimal TotalAmount { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public Guid? DriverId { get; private set; }
    public DateTime? PickedUpAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public string? FailureReason { get; private set; }

    private readonly List<DeliveryAttempt> _deliveryAttempts = new();
    public IReadOnlyCollection<DeliveryAttempt> DeliveryAttempts => _deliveryAttempts.AsReadOnly();

    private Shipment() { }

    private Shipment(
        Guid id,
        Guid orderId,
        string recipientName,
        string recipientPhone,
        AddressSnapshot deliveryAddress,
        decimal totalAmount
    )
        : base(id)
    {
        OrderId = orderId;
        TrackingNumber = TrackingNumber.Generate();
        RecipientName = recipientName;
        RecipientPhone = recipientPhone;
        DeliveryAddress = deliveryAddress;
        TotalAmount = totalAmount;
        Status = ShipmentStatus.Pending;
    }

    public static Shipment Create(
        Guid orderId,
        string recipientName,
        string recipientPhone,
        AddressSnapshot deliveryAddress,
        decimal totalAmount
    )
    {
        if (orderId == Guid.Empty)
            throw new RequiredFieldException(nameof(orderId));

        if (string.IsNullOrWhiteSpace(recipientName))
            throw new RequiredFieldException(nameof(recipientName));

        if (string.IsNullOrWhiteSpace(recipientPhone))
            throw new RequiredFieldException(nameof(recipientPhone));

        if (deliveryAddress is null)
            throw new RequiredFieldException(nameof(deliveryAddress));

        if (totalAmount < 0)
            throw new InvalidValueException("Total amount cannot be negative.");

        var shipment = new Shipment(
            Guid.NewGuid(),
            orderId,
            recipientName.Trim(),
            recipientPhone.Trim(),
            deliveryAddress,
            totalAmount
        );

        return shipment;
    }

    public void AssignDriver(Guid driverId)
    {
        EnsureStatus(nameof(AssignDriver), ShipmentStatus.Pending);

        if (driverId == Guid.Empty)
            throw new RequiredFieldException(nameof(driverId));

        DriverId = driverId;
        Status = ShipmentStatus.Assigned;
    }

    public void PickUp()
    {
        EnsureStatus(nameof(PickUp), ShipmentStatus.Assigned);
        PickedUpAt = DateTime.UtcNow;
        Status = ShipmentStatus.PickedUp;
    }

    public void StartTransit()
    {
        EnsureStatus(nameof(StartTransit), ShipmentStatus.PickedUp);
        Status = ShipmentStatus.InTransit;
    }

    public void MarkDelivered()
    {
        EnsureStatus(nameof(MarkDelivered), ShipmentStatus.InTransit);
        DeliveredAt = DateTime.UtcNow;
        Status = ShipmentStatus.Delivered;
        AddDomainEvent(new ShipmentDeliveredEvent(Id, OrderId, DeliveredAt.Value));
    }

    public void MarkFailed(string reason)
    {
        if (
            Status
            is not (ShipmentStatus.Assigned or ShipmentStatus.PickedUp or ShipmentStatus.InTransit)
        )
            throw new ShipmentNotReadyException("Only an active shipment can be marked as failed.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new RequiredFieldException(nameof(reason));

        FailureReason = reason.Trim();
        Status = ShipmentStatus.Failed;
        _deliveryAttempts.Add(DeliveryAttempt.Create(Id, FailureReason));
    }

    public void Retry()
    {
        EnsureStatus(nameof(Retry), ShipmentStatus.Failed);
        FailureReason = null;
        Status = DriverId.HasValue ? ShipmentStatus.Assigned : ShipmentStatus.Pending;
    }

    public void Cancel()
    {
        if (Status is ShipmentStatus.Delivered or ShipmentStatus.Cancelled)
            throw new InvalidStatusException(
                nameof(Shipment),
                nameof(Cancel),
                Status,
                ShipmentStatus.Pending
            );

        Status = ShipmentStatus.Cancelled;
    }

    public void ChangeStatus(ShipmentStatus status, string? failureReason = null)
    {
        switch (status)
        {
            case ShipmentStatus.PickedUp:
                PickUp();
                break;
            case ShipmentStatus.InTransit:
                StartTransit();
                break;
            case ShipmentStatus.Delivered:
                MarkDelivered();
                break;
            case ShipmentStatus.Failed:
                MarkFailed(failureReason ?? string.Empty);
                break;
            case ShipmentStatus.Pending when Status == ShipmentStatus.Failed:
            case ShipmentStatus.Assigned when Status == ShipmentStatus.Failed:
                Retry();
                break;
            case ShipmentStatus.Cancelled:
                Cancel();
                break;
            default:
                throw new InvalidStatusException(
                    nameof(Shipment),
                    nameof(ChangeStatus),
                    Status,
                    status
                );
        }
    }

    private void EnsureStatus(string operation, ShipmentStatus expectedStatus)
    {
        if (Status != expectedStatus)
            throw new InvalidStatusException(nameof(Shipment), operation, Status, expectedStatus);
    }
}
