using MediatR;

namespace Velora.Application.Features.ShoppingCarts.Commands.AddCartItem;

public sealed record AddCartItemCommand(Guid CustomerId, Guid CartId, Guid ProductId, int Quantity)
    : IRequest;
