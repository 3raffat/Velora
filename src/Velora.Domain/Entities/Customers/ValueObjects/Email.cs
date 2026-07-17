using System.Net.Mail;
using Velora.Domain.Entities.Customers.Errors;

namespace Velora.Domain.Entities.Customers.ValueObjects;

public sealed record Email
{
    public string Value { get; }
    public const byte MaxLength = 255;

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEmailException("Email is required.");

        value = value.Trim();

        if (value.Length > MaxLength)
            throw new InvalidEmailException($"Email cannot exceed {MaxLength} characters.");

        try
        {
            _ = new MailAddress(value);
        }
        catch (FormatException)
        {
            throw new InvalidEmailException("Invalid email format.");
        }

        return new Email(value);
    }
}
