using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Orders.Exceptions;

public sealed class CancellationNotFoundException(Guid orderId)
    : NotFoundException($"Cancellation for order with Id {orderId} was not found.");
