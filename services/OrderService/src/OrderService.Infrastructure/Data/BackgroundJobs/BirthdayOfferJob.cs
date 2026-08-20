using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Domain.Entities.Coupons;
using OrderService.Domain.Entities.Coupons.Enums;
using OrderService.Infrastructure.Extensions;

namespace OrderService.Infrastructure.Data.BackgroundJobs;

public sealed class BirthdayOfferJob(
    IVeloraContext _context,
    ICouponCodeGenerator _codeGenerator,
    IEmailService _emailService,
    ILogger<BirthdayOfferJob> _logger
)
{
    public async Task ExecuteAsync(CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var startOfYear = new DateTime(today.Year, 1, 1);
        var startOfNextYear = startOfYear.AddYears(1);

        var customers = await _context
            .Customers.Where(x =>
                x.IsProfileCompleted
                && x.DateOfBirth.Month == today.Month
                && x.DateOfBirth.Day == today.Day
            )
            .AsNoTracking()
            .Select(x => new
            {
                x.Id,
                x.Email,
                x.FirstName,
            })
            .ToListAsync(ct);

        if (!customers.Any())
        {
            _logger.LogInformation("No customers with birthdays today.");
            return;
        }
        var customerIds = customers.Select(c => c.Id).ToList();

        var alreadySentIds = await _context
            .Coupons.Where(c =>
                customerIds.Contains(c.CustomerId)
                && c.Type == CouponType.Birthday
                && c.CreatedAt >= startOfYear
                && c.CreatedAt < startOfNextYear
            )
            .Select(c => c.CustomerId)
            .ToHashSetAsync(ct);

        var toProcess = customers.Where(c => !alreadySentIds.Contains(c.Id)).ToList();

        if (!toProcess.Any())
        {
            _logger.LogInformation(
                "All eligible customers already received a birthday coupon this year."
            );
            return;
        }

        var newCoupons = new List<(Coupon Coupon, string Email, string FirstName)>();

        foreach (var customer in toProcess)
        {
            var code = _codeGenerator.Generate(CouponType.Birthday);

            var coupon = Coupon.Create(
                customer.Id,
                code,
                25,
                DateTime.UtcNow.AddDays(7),
                CouponType.Birthday
            );

            _context.Coupons.Add(coupon);
            newCoupons.Add((coupon, customer.Email!.Value, customer.FirstName!.Value));

            _logger.LogInformation(
                "Birthday coupon code {CouponCode} generated for customer {CustomerId}",
                code,
                customer.Id
            );
        }
        await _context.SaveChangesAsync(ct);

        foreach (var (coupon, email, firstName) in newCoupons)
        {
            await _emailService.SendBirthdayCouponEmailAsync(
                email,
                firstName,
                coupon.Code,
                coupon.Discount.Amount,
                coupon.ExpiryDate,
                CouponType.Birthday,
                ct
            );
        }
    }
}
