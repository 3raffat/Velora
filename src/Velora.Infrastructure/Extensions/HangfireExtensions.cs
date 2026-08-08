using Hangfire;
using Velora.Infrastructure.Data.BackgroundJobs;

namespace Velora.Infrastructure.Extensions;

public static class HangfireExtensions
{
    public static void RegisterRecurringJobs()
    {
        RecurringJob.AddOrUpdate<MarkAbandonedCartsJob>(
            "mark-abandoned-carts",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Daily
        );

        RecurringJob.AddOrUpdate<CleanupExpiredUnusedCouponsJob>(
            "cleanup-expired-unused-coupons",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Hourly
        );
    }
}
