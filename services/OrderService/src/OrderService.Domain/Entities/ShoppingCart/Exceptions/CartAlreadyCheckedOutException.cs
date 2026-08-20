using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.ShoppingCart.Exceptions;

public sealed class CartAlreadyCheckedOutException : DomainException
{
    public CartAlreadyCheckedOutException()
        : base("Cannot modify a checked out cart.") { }
}
