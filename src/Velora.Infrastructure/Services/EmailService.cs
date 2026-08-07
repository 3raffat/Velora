using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Models;
using Velora.Infrastructure.Common;

namespace Velora.Infrastructure.Services;

public sealed class EmailService(IConfiguration _cfg, ILogger<EmailService> _logger) : IEmailService
{
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
