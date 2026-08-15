using Velora.Domain.Common;

namespace Velora.Domain.Entities.Orders.Events;

public sealed record OrderCancelledEvent(Guid OrderId, Guid CancellationId, Guid CustomerId)
    : DomainEvent;
