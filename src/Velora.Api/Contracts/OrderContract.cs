using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Api.Contracts;

public sealed record CheckoutRequest(
    Guid CartId,
    Guid BillingAddressId,
    Guid ShippingAddressId,
    PaymentMethod PaymentMethod,
    decimal ShippingCost = 0,
    string? PromoCode = null
);

public sealed record CancelOrderRequest(string Reason);

public sealed record ApproveCancellationRequest(decimal? CancellationCharges = null);

public sealed record RejectCancellationRequest(string Remarks);

public sealed record CompleteRefundRequest(string TransactionId);

public sealed record RejectRefundRequest(string Reason);

public sealed record RequestCancellationRequest(string Reason);
