using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.ShoppingCart.Exceptions;

public sealed class InvalidQuantityException : DomainException
{
    public InvalidQuantityException()
        : base("Quantity must be at least 1.") { }

    public InvalidQuantityException(int currentQuantity, int amount)
        : base(
            $"Cannot decrease quantity from {currentQuantity} by {amount}. Quantity cannot drop below 1. Remove the item instead."
        ) { }
}
