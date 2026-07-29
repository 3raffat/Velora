using MediatR;

namespace Velora.Application.Features.ShoppingCarts.Commands.Checkout;

public sealed record CheckoutCartCommand(
    Guid CustomerId,
    Guid CartId,
    Guid ShippingAddressId,
    Guid BillingAddressId,
    decimal ShippingCost
) : IRequest;
