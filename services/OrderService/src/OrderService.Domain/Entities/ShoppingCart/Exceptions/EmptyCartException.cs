using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.ShoppingCart.Exceptions;

public sealed class EmptyCartException : DomainException
{
    public EmptyCartException()
        : base("Cart is already empty.") { }
}
