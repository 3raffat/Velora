using System.Net.Mail;
using Microsoft.AspNetCore.Identity;

namespace Velora.Infrastructure.Services.Models;

public sealed class AppUser : IdentityUser<Guid>
{
    private AppUser() { }

    private AppUser(string username, string email)
    {
        this.Id = Guid.NewGuid();
        this.UserName = username;
        this.Email = email;
    }

    public static AppUser Create(string username, string email)
    {
        Validate(username, email);

        return new AppUser(username, email);
    }

    private static void Validate(string username, string email)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.", nameof(username));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        try
        {
            _ = new MailAddress(email);
        }
        catch
        {
            throw new ArgumentException("Invalid email address.", nameof(email));
        }
    }

    public string GetIdString() => Id.ToString();
}
