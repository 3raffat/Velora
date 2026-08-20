using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.ShoppingCarts.Exceptions;

public sealed class CartNotFoundException(Guid id)
    : NotFoundException($"Cart with Id {id} was not found.") { }
