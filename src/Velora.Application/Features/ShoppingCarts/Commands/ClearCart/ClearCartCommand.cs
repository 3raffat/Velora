using MediatR;

namespace Velora.Application.Features.ShoppingCarts.Commands.ClearCart;

public sealed record ClearCartCommand(Guid CustomerId, Guid CartId) : IRequest;
