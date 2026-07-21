using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Extensions;
using Velora.Application.Common.Interfaces;
using Velora.Domain.Common;

namespace Velora.Infrastructure.Data.Interceptors;

public sealed class AuditableEntityInterceptor(
    ILogger<AuditableEntityInterceptor> _logger,
    TimeProvider _timeProvider,
    ICurrentUser _user
) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        if (eventData.Context is null)
        {
            _logger.LogWarning("DbContext is null in SavingChanges");
            return base.SavingChanges(eventData, result);
        }

        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        if (eventData.Context is null)
        {
            _logger.LogWarning("DbContext is null in SavingChanges");
            return base.SavingChangesAsync(eventData, result);
        }

        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext context)
    {
        var user = _user.GetCurrentUserOrSystem();

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.MarkAsCreated(user.Id);
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.MarkAsUpdated(user.Id);
            }
        }
    }
}
