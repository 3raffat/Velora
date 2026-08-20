using OrderService.Domain.Common;

namespace OrderService.Domain.Entities.Customers.Events;

public sealed record CustomerProfileCompletedEvent(Guid CustomerId) : DomainEvent;
