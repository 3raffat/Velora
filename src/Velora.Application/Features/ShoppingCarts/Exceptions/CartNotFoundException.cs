using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.ShoppingCarts.Exceptions;

public sealed class CartNotFoundException(Guid id)
    : NotFoundException($"Cart with Id {id} was not found.") { }
