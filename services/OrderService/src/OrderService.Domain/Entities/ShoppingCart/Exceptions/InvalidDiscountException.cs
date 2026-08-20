using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.ShoppingCart.Exceptions;

public sealed class InvalidDiscountException : DomainException
{
    public InvalidDiscountException(decimal discount)
        : base($"Discount '{discount}' cannot be negative.") { }

    public InvalidDiscountException(decimal discount, decimal total)
        : base($"Discount '{discount}' cannot exceed the item's total price '{total}'.") { }
}
