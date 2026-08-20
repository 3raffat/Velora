using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.ShoppingCart.Exceptions;

public sealed class CartItemNotFoundException : DomainException
{
    public CartItemNotFoundException(Guid productId)
        : base($"Cart item with product ID '{productId}' was not found.") { }
}
