using DeliveryService.Domain.Common.Exceptions;

namespace DeliveryService.Domain.Entities.Shipments.Exceptions;

public sealed class ShipmentNotReadyException(string message) : DomainException(message);
