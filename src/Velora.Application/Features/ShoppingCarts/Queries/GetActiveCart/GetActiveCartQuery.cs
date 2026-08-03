using MediatR;
using Velora.Application.Features.ShoppingCarts.Dtos;

namespace Velora.Application.Features.ShoppingCarts.Queries.GetActiveCart;

public sealed record GetActiveCartQuery(Guid CustomerId) : IRequest<CartDto?>;
