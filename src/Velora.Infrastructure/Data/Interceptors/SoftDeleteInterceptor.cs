using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Extensions;
using Velora.Application.Common.Interfaces;
using Velora.Domain.Common;

namespace Velora.Infrastructure.Data.Interceptors;

public sealed class SoftDeleteInterceptor(
    ILogger<SoftDeleteInterceptor> _logger,
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

        ProcessSoftDeletes(eventData.Context);

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

        ProcessSoftDeletes(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void ProcessSoftDeletes(DbContext context)
    {
        var user = _user.GetCurrentUserOrSystem();
        var time = _timeProvider.GetUtcNow().DateTime;

        foreach (var entry in context.ChangeTracker.Entries<SoftDeletableEntity>())
        {
            if (EntityState.Deleted == entry.State)
            {
                entry.State = EntityState.Modified;
                entry.Entity.MarkAsDeleted(user.Id);

                _logger.LogInformation(
                    "Soft deleted auditable entity {entity} with user id {userid} at {time}",
                    entry.Entity.GetType().Name,
                    user.Id,
                    time
                );
            }
        }
    }
}
