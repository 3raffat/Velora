using Hangfire;
using OrderService.Infrastructure.Data.BackgroundJobs;

namespace OrderService.Infrastructure.Extensions;

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

        RecurringJob.AddOrUpdate<BirthdayOfferJob>(
            "birthday-offers",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Daily(9)
        );
    }
}
