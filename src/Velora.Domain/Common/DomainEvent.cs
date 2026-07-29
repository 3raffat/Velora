using MediatR;

namespace Velora.Domain.Common;

public abstract record DomainEvent : INotification { }
