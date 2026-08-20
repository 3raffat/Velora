using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Domain.Entities.ShoppingCart.Enums;

namespace OrderService.Infrastructure.Data.BackgroundJobs;

public class MarkAbandonedCartsJob(IVeloraContext _context, ILogger<MarkAbandonedCartsJob> _logger)
{
    public async Task ExecuteAsync(CancellationToken ct)
    {
        var threshold = DateTime.UtcNow.AddDays(-30);

        var affected = await _context
            .Carts.Where(c =>
                c.Status == CartStatus.Active && c.UpdatedAt < threshold && c.CartItems.Any()
            )
            .ExecuteUpdateAsync(u => u.SetProperty(c => c.Status, CartStatus.Abandoned), ct);

        _logger.LogInformation("Marked {Count} carts as abandoned", affected);
    }
}
