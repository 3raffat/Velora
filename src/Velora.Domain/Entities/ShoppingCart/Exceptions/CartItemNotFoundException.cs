using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.ShoppingCart.Exceptions;

public sealed class CartItemNotFoundException : DomainException
{
    public CartItemNotFoundException(Guid productId)
        : base($"Cart item with product ID '{productId}' was not found.") { }
}
