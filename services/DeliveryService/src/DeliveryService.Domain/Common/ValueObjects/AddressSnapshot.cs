using System.Text.Json.Serialization;
using DeliveryService.Domain.Common.Exceptions;

namespace DeliveryService.Domain.Common.ValueObjects;

public sealed record AddressSnapshot
{
    public string AddressLine1 { get; }
    public string? AddressLine2 { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }

    [JsonConstructor]
    public AddressSnapshot(
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string country
    )
    {
        AddressLine1 = Required(addressLine1, nameof(addressLine1));
        AddressLine2 = string.IsNullOrWhiteSpace(addressLine2) ? null : addressLine2.Trim();
        City = Required(city, nameof(city));
        State = Required(state, nameof(state));
        Country = Required(country, nameof(country));
    }

    public static AddressSnapshot Create(
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string country
    )
    {
        return new AddressSnapshot(addressLine1, addressLine2, city, state, country);
    }

    private static string Required(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new RequiredFieldException(fieldName);

        return value.Trim();
    }
}
