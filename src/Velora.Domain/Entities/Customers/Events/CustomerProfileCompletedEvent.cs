using Velora.Domain.Common;

namespace Velora.Domain.Entities.Customers.Events;

public sealed record CustomerProfileCompletedEvent(Guid CustomerId) : DomainEvent;
