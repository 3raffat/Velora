using DeliveryService.Domain.Common;

namespace DeliveryService.Domain.Entities.Shipments.Events;

public sealed record ShipmentCreatedEvent(Guid ShipmentId, Guid OrderId, string TrackingNumber)
    : DomainEvent;
