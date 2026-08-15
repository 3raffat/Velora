using MediatR;
using Velora.Application.Features.Orders.Dtos;

namespace Velora.Application.Features.Orders.Queries.GetCancellationByOrderId;

public sealed record GetCancellationByOrderIdQuery(Guid CustomerId, Guid OrderId)
    : IRequest<CancellationDto>;
