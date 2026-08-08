using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Customers.Exceptions;
using Velora.Domain.Entities.Coupons;
using Velora.Domain.Entities.Coupons.Enums;
using Velora.Domain.Entities.Customers.Events;

namespace Velora.Application.Features.Customers.Events;

public sealed class CustomerProfileCompletedEventHandler(
    IVeloraContext _context,
    ICouponCodeGenerator _codeGenerator,
    IEmailService _emailService,
    ILogger<CustomerProfileCompletedEventHandler> _logger
) : INotificationHandler<CustomerProfileCompletedEvent>
{
    public async Task Handle(CustomerProfileCompletedEvent notification, CancellationToken ct)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(
            c => c.Id == notification.CustomerId,
            ct
        );

        if (customer is null)
            throw new CustomerNotFoundException(notification.CustomerId);

        var code = _codeGenerator.Generate(CouponType.Welcome);

        var coupon = Coupon.Create(
            notification.CustomerId,
            code,
            10,
            DateTime.UtcNow.AddDays(30),
            CouponType.Welcome
        );

        await _context.Coupons.AddAsync(coupon, ct);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Welcome coupon code {CouponCode} generated for customer {CustomerId}",
            code,
            notification.CustomerId
        );

        await _emailService.SendCouponEmailAsync(
            customer.Email!.Value,
            customer.FirstName!.Value,
            code,
            10,
            coupon.ExpiryDate,
            CouponType.Welcome,
            ct
        );
    }
}
