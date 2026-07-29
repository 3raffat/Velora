using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.ShoppingCart.Exceptions;

public sealed class EmptyCartException : DomainException
{
    public EmptyCartException()
        : base("Cart is already empty.") { }
}
