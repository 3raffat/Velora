using System.Net.Mail;
using DeliveryService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace DeliveryService.Infrastructure.Services.Models;

public sealed class AppUser : IdentityUser<Guid>
{
    private AppUser() { }

    private AppUser(string username, string email)
    {
        Id = Guid.NewGuid();
        UserName = username;
        Email = email;
        EmailConfirmed = true;
    }

    public bool IsActive { get; private set; } = true;

    public void SetActive(bool isActive) => IsActive = isActive;

    public static AppUser Create(string username, string email)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidRequestException("Username is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidRequestException("Email is required.");

        _ = new MailAddress(email);
        return new AppUser(username.Trim(), email.Trim());
    }
}
