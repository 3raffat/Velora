using Velora.Domain.Entities.Customers.Errors;

namespace Velora.Domain.Common.ValueObjects;

public sealed record Name
{
    public string Value { get; }
    private const int MaxLength = 20;
    private const int MinLength = 3;

    private Name(string value)
    {
        Value = value;
    }

    public static Name Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidNameException("name is required");

        var trimmed = value.Trim();

        if (trimmed.Length > MaxLength)
            throw new InvalidNameException($"Name cannot exceed {MaxLength} characters");

        if (trimmed.Length < MinLength)
            throw new InvalidNameException($"Name must be at least {MinLength} characters long.");

        return new Name(trimmed);
    }
}
