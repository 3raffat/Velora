using OrderService.Domain.Entities.Products.Exceptions;

namespace OrderService.Domain.Entities.Products.ValueObjects;

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
            throw new InvalidRatingException(value);

        return new Rating(value);
    }
}
