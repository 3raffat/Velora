using MediatR;

namespace Velora.Application.Features.ShoppingCarts.Commands.UpdateItemQuantity;

public sealed record UpdateItemQuantityCommand(
    Guid CustomerId,
    Guid CartId,
    Guid ProductId,
    int Quantity
) : IRequest;
