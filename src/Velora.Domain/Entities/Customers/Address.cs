using Velora.Domain.Common;
using Velora.Domain.Entities.Customers.Constants;

namespace Velora.Domain.Entities.Customers;

public sealed class Address : BaseEntity
{
    public string AddressLine1 { get; private set; } = string.Empty;
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;

    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;

    private Address() { }

    private Address(
        Guid id,
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string country,
        Guid customerId
    )
        : base(id)
    {
        AddressLine1 = addressLine1.Trim();
        AddressLine2 = addressLine2?.Trim();
        City = city.Trim();
        State = state.Trim();
        Country = country.Trim();
        CustomerId = customerId;
    }

    public static Address Create(
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string country,
        Guid customerId
    )
    {
        ValidateFields(addressLine1, addressLine2, city, state, country);
        ValidateCustomerId(customerId);

        return new Address(
            Guid.NewGuid(),
            addressLine1,
            addressLine2,
            city,
            state,
            country,
            customerId
        );
    }

    public void Update(
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string country
    )
    {
        ValidateFields(addressLine1, addressLine2, city, state, country);

        AddressLine1 = addressLine1.Trim();
        AddressLine2 = addressLine2?.Trim();
        City = city.Trim();
        State = state.Trim();
        Country = country.Trim();
    }

    private static void ValidateFields(
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string country
    )
    {
        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new ArgumentException("Address Line 1 is required.", nameof(addressLine1));

        if (addressLine1.Length > AddressConstraints.AddressLineMaxLength)
            throw new ArgumentException(
                $"Address Line 1 cannot exceed {AddressConstraints.AddressLineMaxLength} characters.",
                nameof(addressLine1)
            );

        if (
            addressLine2 is not null
            && addressLine2.Length > AddressConstraints.AddressLineMaxLength
        )
            throw new ArgumentException(
                $"Address Line 2 cannot exceed {AddressConstraints.AddressLineMaxLength} characters.",
                nameof(addressLine2)
            );

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required.", nameof(city));

        if (city.Length > AddressConstraints.CityMaxLength)
            throw new ArgumentException(
                $"City cannot exceed {AddressConstraints.CityMaxLength} characters.",
                nameof(city)
            );

        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State is required.", nameof(state));

        if (state.Length > AddressConstraints.StateMaxLength)
            throw new ArgumentException(
                $"State cannot exceed {AddressConstraints.StateMaxLength} characters.",
                nameof(state)
            );

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required.", nameof(country));

        if (country.Length > AddressConstraints.CountryMaxLength)
            throw new ArgumentException(
                $"Country cannot exceed {AddressConstraints.CountryMaxLength} characters.",
                nameof(country)
            );
    }

    private static void ValidateCustomerId(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer Id is required.", nameof(customerId));
    }
}
