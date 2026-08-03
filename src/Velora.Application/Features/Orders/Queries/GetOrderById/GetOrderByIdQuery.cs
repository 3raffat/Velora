using MediatR;
using Velora.Application.Features.Orders.Dtos;

namespace Velora.Application.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid CustomerId, Guid OrderId) : IRequest<OrderDetailDto>;
