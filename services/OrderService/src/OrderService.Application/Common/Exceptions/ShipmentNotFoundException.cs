namespace OrderService.Application.Common.Exceptions;

public sealed class ShipmentNotFoundException(Guid orderId)
    : NotFoundException($"Shipment for order '{orderId}' was not found.");
