using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.ShoppingCarts.Exceptions;

public sealed class EmptyCartException : ConflictException
{
    public EmptyCartException()
        : base("Cart is empty.") { }
}
