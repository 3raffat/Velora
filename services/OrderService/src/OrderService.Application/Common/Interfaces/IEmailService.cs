using OrderService.Application.Features.Orders.Dtos;
using OrderService.Domain.Entities.Coupons.Enums;

namespace OrderService.Application.Common.Interfaces;

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

    Task SendOrderConfirmationEmailAsync(
        string email,
        OrderDetailDto order,
        CancellationToken cancellationToken = default
    );

    Task SendCancellationConfirmationEmailAsync(
        string email,
        OrderDetailDto order,
        CancellationDto cancellation,
        CancellationToken cancellationToken = default
    );

    Task SendRefundConfirmationEmailAsync(
        string email,
        OrderDetailDto order,
        RefundDto refund,
        CancellationToken cancellationToken = default
    );
}
