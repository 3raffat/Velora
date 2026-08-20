using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Orders.Exceptions;

public sealed class CancellationAlreadyPendingException : ConflictException
{
    public CancellationAlreadyPendingException(Guid orderId)
        : base($"A cancellation request is already pending for order '{orderId}'.") { }
}
