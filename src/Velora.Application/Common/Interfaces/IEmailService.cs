using Velora.Domain.Entities.Coupons.Enums;

namespace Velora.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(
        string to,
        string userName,
        string confirmationLink,
        CancellationToken ct
    );
    Task SendCouponEmailAsync(
        string email,
        string customerName,
        string couponCode,
        decimal discount,
        DateTime? expiresAt,
        CouponType couponType,
        CancellationToken cancellationToken = default
    );

    Task SendBirthdayCouponEmailAsync(
        string email,
        string customerName,
        string couponCode,
        decimal discount,
        DateTime expiresAt,
        CouponType couponType,
        CancellationToken cancellationToken = default
    );
}
