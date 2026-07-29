using MediatR;

namespace Velora.Application.Features.ShoppingCarts.Commands.RemoveCartItem;

public sealed record RemoveCartItemCommand(Guid CustomerId, Guid CartId, Guid ProductId) : IRequest;
