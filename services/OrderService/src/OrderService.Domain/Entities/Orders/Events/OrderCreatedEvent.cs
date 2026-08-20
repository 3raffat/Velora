using OrderService.Domain.Common;

namespace OrderService.Domain.Entities.Orders.Events;

public sealed record OrderCreatedEvent(Guid OrderId, Guid CustomerId) : DomainEvent;
