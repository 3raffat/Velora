using OrderService.Domain.Entities.Orders.ValueObjects;

namespace OrderService.Application.Common.Models;

public sealed record CreateShipmentRequest(
    Guid OrderId,
    string CustomerName,
    string CustomerPhone,
    AddressSnapshot ShippingAddress,
    decimal TotalAmount
);
