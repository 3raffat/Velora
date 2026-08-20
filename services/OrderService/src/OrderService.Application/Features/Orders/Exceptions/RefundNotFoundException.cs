using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Orders.Exceptions;

public sealed class RefundNotFoundException(Guid orderId)
    : NotFoundException($"Refund for order with Id {orderId} was not found.");
