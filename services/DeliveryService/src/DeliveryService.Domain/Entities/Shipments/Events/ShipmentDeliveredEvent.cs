using DeliveryService.Domain.Common;

namespace DeliveryService.Domain.Entities.Shipments.Events;

public sealed record ShipmentDeliveredEvent(Guid ShipmentId, Guid OrderId, DateTime DeliveredAt)
    : DomainEvent;
