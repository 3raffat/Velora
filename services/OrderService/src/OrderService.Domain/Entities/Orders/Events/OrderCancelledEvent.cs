using OrderService.Domain.Common;

namespace OrderService.Domain.Entities.Orders.Events;

public sealed record OrderCancelledEvent(Guid OrderId, Guid CancellationId, Guid CustomerId)
    : DomainEvent;
