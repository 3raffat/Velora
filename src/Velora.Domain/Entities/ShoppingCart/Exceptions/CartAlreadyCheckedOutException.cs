using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.ShoppingCart.Exceptions;

public sealed class CartAlreadyCheckedOutException : DomainException
{
    public CartAlreadyCheckedOutException()
        : base("Cannot modify a checked out cart.") { }
}
