using Velora.Domain.Entities.Orders.ValueObjects;

namespace Velora.Application.Common.Models;

public sealed record CreateShipmentRequest(
    Guid OrderId,
    string CustomerName,
    string CustomerPhone,
    AddressSnapshot ShippingAddress,
    decimal TotalAmount
);
