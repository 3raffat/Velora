namespace Velora.Domain.Entities.Products.ValueObjects;

public sealed record Rating
{
    public byte Value { get; }

    private Rating(byte value)
    {
        Value = value;
    }

    public static Rating Create(byte value)
    {
        if (value < 1 || value > 5)
            throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 1 and 5.");

        return new Rating(value);
    }
}
