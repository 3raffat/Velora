using DeliveryService.Domain.Common.Exceptions;

namespace DeliveryService.Domain.Entities.Shipments.ValueObjects;

public sealed record TrackingNumber
{
    public string Value { get; }

    private TrackingNumber(string value)
    {
        Value = value;
    }

    public static TrackingNumber Generate()
    {
        return new TrackingNumber(
            $"VEL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..22].ToUpperInvariant()
        );
    }

    public static TrackingNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new RequiredFieldException(nameof(value));

        return new TrackingNumber(value.Trim().ToUpperInvariant());
    }

    public override string ToString() => Value;
}
