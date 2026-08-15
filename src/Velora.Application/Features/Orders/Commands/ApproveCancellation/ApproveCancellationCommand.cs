using MediatR;

namespace Velora.Application.Features.Orders.Commands.ApproveCancellation;

public sealed record ApproveCancellationCommand(
    Guid ProcessedBy,
    Guid OrderId,
    decimal? CancellationCharges = null
) : IRequest;
