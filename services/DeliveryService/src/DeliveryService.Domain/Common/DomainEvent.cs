using MediatR;

namespace DeliveryService.Domain.Common;

public abstract record DomainEvent : INotification { }
