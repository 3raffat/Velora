using MediatR;
using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Application.Features.ShoppingCarts.Commands.Checkout;

public sealed record CheckoutCartCommand(
    Guid CustomerId,
    Guid CartId,
    Guid ShippingAddressId,
    Guid? BillingAddressId,
    PaymentMethod PaymentMethod,
    decimal ShippingCost = 0,
    string? PromoCode = null
) : IRequest<Guid>;
