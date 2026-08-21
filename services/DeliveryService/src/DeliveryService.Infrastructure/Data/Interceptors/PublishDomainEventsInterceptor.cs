using DeliveryService.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DeliveryService.Infrastructure.Data.Interceptors;

public sealed class PublishDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken ct = default
    )
    {
        if (eventData.Context is not null)
            await PublishDomainEvents(eventData.Context, ct);

        return await base.SavedChangesAsync(eventData, result, ct);
    }

    private async Task PublishDomainEvents(DbContext context, CancellationToken ct)
    {
        var entities = context
            .ChangeTracker.Entries<BaseEntity>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Count > 0)
            .ToList();

        var events = entities.SelectMany(entity => entity.DomainEvents).ToList();
        foreach (var entity in entities)
            entity.ClearDomainEvents();

        foreach (var domainEvent in events)
            await mediator.Publish(domainEvent, ct);
    }
}
