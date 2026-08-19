using DeliveryService.Entities.Shipments;

namespace DeliveryService.Contracts;

public sealed record CreateShipmentRequest(
    Guid OrderId,
    string CustomerName,
    string CustomerPhone,
    AddressSnapshot ShippingAddress,
    decimal TotalAmount
);
