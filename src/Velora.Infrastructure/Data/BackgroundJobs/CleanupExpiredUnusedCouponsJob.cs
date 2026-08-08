using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;

namespace Velora.Infrastructure.Data.BackgroundJobs;

public sealed class CleanupExpiredUnusedCouponsJob(
    IVeloraContext _context,
    ILogger<CleanupExpiredUnusedCouponsJob> _logger
)
{
    public async Task ExecuteAsync(CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var deletedCount = await _context
            .Coupons.Where(c => !c.IsUsed && c.ExpiryDate <= now)
            .ExecuteDeleteAsync(ct);

        _logger.LogInformation("Expired {Count} coupons", deletedCount);
    }
}
