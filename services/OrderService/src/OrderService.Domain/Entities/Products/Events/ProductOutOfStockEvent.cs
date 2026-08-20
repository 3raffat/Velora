using OrderService.Domain.Common;

namespace OrderService.Domain.Entities.Products.Events;

public sealed record ProductOutOfStockEvent(Guid ProductId) : DomainEvent;
