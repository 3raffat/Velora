using Velora.Domain.Common;

namespace Velora.Domain.Entities.Products.Events;

public sealed record ProductBackInStockEvent(Guid ProductId) : DomainEvent;
