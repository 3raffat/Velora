using Velora.Domain.Entities.Customers;
using Velora.Domain.Entities.ShoppingCart;

namespace Velora.Application.Features.ShoppingCarts.Dtos;

public record CustomerDto(
    IEnumerable<CartItem>? CartItems,
    Cart? Cart,
    Address? ShippingAddress,
    Address BillingAddress
);
