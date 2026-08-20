using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;
using OrderService.Application.Features.Orders.Dtos;
using OrderService.Domain.Entities.Coupons.Enums;
using OrderService.Infrastructure.Common;

namespace OrderService.Infrastructure.Services;

public sealed class EmailService(IConfiguration _cfg, ILogger<EmailService> _logger) : IEmailService
{
    public async Task SendCancellationConfirmationEmailAsync(
        string email,
        OrderDetailDto order,
        CancellationDto cancellation,
        CancellationToken cancellationToken = default
    )
    {
        var subject = $"Order #{order.OrderNumber} - Cancellation Approved";

        await SendAsync(
            new EmailMessage(
                email,
                $"Order #{order.OrderNumber} - Cancellation Approved",
                EmailTemplates.CancellationConfirmationBody(order, cancellation)
            ),
            cancellationToken
        );
    }

    public async Task SendRefundConfirmationEmailAsync(
        string email,
        OrderDetailDto order,
        RefundDto refund,
        CancellationToken cancellationToken = default
    )
    {
        await SendAsync(
            new EmailMessage(
                email,
                $"Order #{order.OrderNumber} - Refund {refund.Status}",
                EmailTemplates.RefundConfirmationBody(order, refund)
            ),
            cancellationToken
        );
    }

    public async Task SendConfirmationEmailAsync(
        string to,
        string userName,
        string confirmationLink,
        CancellationToken ct
    )
    {
        await SendAsync(
            new EmailMessage(
                to,
                "Confirm your email",
                EmailTemplates.ConfirmationEmailBody(userName, confirmationLink)
            ),
            ct
        );
    }

    public async Task SendCouponEmailAsync(
        string email,
        string customerName,
        string couponCode,
        decimal discount,
        DateTime? expiresAt,
        CouponType couponType,
        CancellationToken ct = default
    )
    {
        await SendAsync(
            new EmailMessage(
                email,
                "Your Coupon Code",
                EmailTemplates.CouponBody(customerName, couponCode, discount, expiresAt)
            ),
            ct
        );
    }

    public async Task SendBirthdayCouponEmailAsync(
        string email,
        string customerName,
        string couponCode,
        decimal discount,
        DateTime expiresAt,
        CouponType couponType,
        CancellationToken cancellationToken = default
    )
    {
        await SendAsync(
            new EmailMessage(
                email,
                "Happy Birthday! Here's a special gift for you",
                EmailTemplates.BirthdayCouponBody(customerName, couponCode, discount, expiresAt)
            ),
            cancellationToken
        );
    }

    public async Task SendOrderConfirmationEmailAsync(
        string email,
        OrderDetailDto order,
        CancellationToken cancellationToken = default
    )
    {
        await SendAsync(
            new EmailMessage(
                email,
                $"Order #{order.OrderNumber} Confirmed",
                EmailTemplates.ToEmailHtml(order)
            ),
            cancellationToken
        );
    }

    public async Task SendAsync(EmailMessage message, CancellationToken ct = default)
    {
        var emailSettings = _cfg.GetSection("EmailSettings");

        var email = new MimeKit.MimeMessage();

        var from = new MailboxAddress(emailSettings["DisplayName"], emailSettings["Email"]!);
        var to = MailboxAddress.Parse(message.To);

        email.From.Add(from);
        email.To.Add(to);
        email.Subject = $"{message.Subject} :{message.To}";

        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Body };

        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        var port = int.Parse(emailSettings["Port"]!);

        await smtp.ConnectAsync(
            emailSettings["Host"]!,
            port,
            MailKit.Security.SecureSocketOptions.StartTls,
            ct
        );
        await smtp.AuthenticateAsync(emailSettings["Email"]!, emailSettings["Password"]!, ct);

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true, ct);

        _logger.LogInformation("Email sent to {Recipient}.", message.To);
    }
}
