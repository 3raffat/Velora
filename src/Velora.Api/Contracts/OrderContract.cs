using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Api.Contracts;

public sealed record CheckoutRequest(
    Guid CartId,
    Guid BillingAddressId,
    Guid ShippingAddressId,
    PaymentMethod PaymentMethod,
    decimal ShippingCost = 0
);

public sealed record CancelOrderRequest(string Reason);
