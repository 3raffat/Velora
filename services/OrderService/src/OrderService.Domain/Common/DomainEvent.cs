using MediatR;

namespace OrderService.Domain.Common;

public abstract record DomainEvent : INotification { }
