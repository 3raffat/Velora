using MediatR;
using OrderService.Application.Features.Orders.Dtos;

namespace OrderService.Application.Features.Orders.Queries.GetCancellationByOrderId;

public sealed record GetCancellationByOrderIdQuery(Guid CustomerId, Guid OrderId)
    : IRequest<CancellationDto>;
