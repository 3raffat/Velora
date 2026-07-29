using PhoneNumbers;
using Velora.Domain.Entities.Customers;
using Velora.Domain.Entities.Customers.Exceptions;

namespace Velora.Domain.Entities.Orders.ValueObjects;

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

    public static AddressSnapshot From(Address address)
    {
        return new AddressSnapshot(
            address.AddressLine1,
            address.AddressLine2,
            address.City,
            address.State,
            address.Country
        );
    }
}
