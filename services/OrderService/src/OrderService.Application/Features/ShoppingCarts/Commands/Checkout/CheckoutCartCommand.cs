using MediatR;
using OrderService.Domain.Entities.Orders.Enums;

namespace OrderService.Application.Features.ShoppingCarts.Commands.Checkout;

public sealed record CheckoutCartCommand(
    Guid CustomerId,
    Guid CartId,
    Guid ShippingAddressId,
    Guid? BillingAddressId,
    PaymentMethod PaymentMethod,
    decimal ShippingCost = 0,
    string? PromoCode = null
) : IRequest<Guid>;
