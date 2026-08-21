namespace DeliveryService.Domain.Entities.Shipments.Enums;

public enum ShipmentStatus
{
    Pending,
    Assigned,
    PickedUp,
    InTransit,
    Delivered,
    Failed,
    Cancelled,
}
