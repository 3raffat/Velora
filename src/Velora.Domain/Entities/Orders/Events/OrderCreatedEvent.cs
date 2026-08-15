using Velora.Domain.Common;

namespace Velora.Domain.Entities.Orders.Events;

public sealed record OrderCreatedEvent(Guid OrderId, Guid CustomerId) : DomainEvent;
