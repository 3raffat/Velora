using MediatR;
using OrderService.Application.Features.ShoppingCarts.Dtos;

namespace OrderService.Application.Features.ShoppingCarts.Queries.GetActiveCart;

public sealed record GetActiveCartQuery(Guid CustomerId) : IRequest<CartDto?>;
