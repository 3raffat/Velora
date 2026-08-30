using MediatR;
using OrderService.Application.Features.Orders.Dtos;

namespace OrderService.Application.Features.Orders.Queries.GetAllCancellations;

public sealed record GetAllCancellationsQuery : IRequest<IReadOnlyCollection<CancellationDto>>;
