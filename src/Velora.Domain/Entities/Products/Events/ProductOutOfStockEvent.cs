using Velora.Domain.Common;

namespace Velora.Domain.Entities.Products.Events;

public sealed record ProductOutOfStockEvent(Guid ProductId) : DomainEvent;
