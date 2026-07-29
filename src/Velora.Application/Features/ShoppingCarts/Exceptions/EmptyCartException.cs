using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.ShoppingCarts.Exceptions;

public sealed class EmptyCartException : ConflictException
{
    public EmptyCartException()
        : base("Cart is empty.") { }
}
