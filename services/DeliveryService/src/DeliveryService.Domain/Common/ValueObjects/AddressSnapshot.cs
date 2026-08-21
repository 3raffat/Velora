using DeliveryService.Domain.Common.Exceptions;

namespace DeliveryService.Domain.Common.ValueObjects;

public sealed record AddressSnapshot
{
    public string AddressLine1 { get; }
    public string? AddressLine2 { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }

    private AddressSnapshot(
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string country
    )
    {
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        State = state;
        Country = country;
    }

    public static AddressSnapshot Create(
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string country
    )
    {
        return new AddressSnapshot(
            Required(addressLine1, nameof(addressLine1)),
            string.IsNullOrWhiteSpace(addressLine2) ? null : addressLine2.Trim(),
            Required(city, nameof(city)),
            Required(state, nameof(state)),
            Required(country, nameof(country))
        );
    }

    private static string Required(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new RequiredFieldException(fieldName);

        return value.Trim();
    }
}
