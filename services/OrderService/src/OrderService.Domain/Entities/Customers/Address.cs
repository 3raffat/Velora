using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;
using OrderService.Domain.Entities.Customers.Constants;
using OrderService.Domain.Entities.Customers.Exceptions;

namespace OrderService.Domain.Entities.Customers;

public sealed class Address : BaseEntity
{
    public string AddressLine1 { get; private set; } = string.Empty;
    public string AddressLine2 { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;

    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;

    private Address() { }

    private Address(
        Guid id,
        string addressLine1,
        string addressLine2,
        string city,
        string state,
        string country,
        Guid customerId
    )
        : base(id)
    {
        AddressLine1 = addressLine1.Trim();
        AddressLine2 = addressLine2.Trim();
        City = city.Trim();
        State = state.Trim();
        Country = country.Trim();
        CustomerId = customerId;
    }

    internal static Address Create(
        string addressLine1,
        string addressLine2,
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

    internal void Update(
        string addressLine1,
        string addressLine2,
        string city,
        string state,
        string country
    )
    {
        ValidateFields(addressLine1, addressLine2, city, state, country);

        AddressLine1 = addressLine1.Trim();
        AddressLine2 = addressLine2.Trim();
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
            throw new InvalidAddressException("Address Line 1 is required.");

        if (addressLine1.Length > AddressConstraints.AddressLineMaxLength)
            throw new InvalidAddressException(
                $"Address Line 1 cannot exceed {AddressConstraints.AddressLineMaxLength} characters."
            );

        if (
            addressLine2 is not null
            && addressLine2.Length > AddressConstraints.AddressLineMaxLength
        )
            throw new InvalidAddressException(
                $"Address Line 2 cannot exceed {AddressConstraints.AddressLineMaxLength} characters."
            );

        if (string.IsNullOrWhiteSpace(city))
            throw new InvalidAddressException("City is required.");

        if (city.Length > AddressConstraints.CityMaxLength)
            throw new InvalidAddressException(
                $"City cannot exceed {AddressConstraints.CityMaxLength} characters."
            );

        if (string.IsNullOrWhiteSpace(state))
            throw new InvalidAddressException("State is required.");

        if (state.Length > AddressConstraints.StateMaxLength)
            throw new InvalidAddressException(
                $"State cannot exceed {AddressConstraints.StateMaxLength} characters."
            );

        if (string.IsNullOrWhiteSpace(country))
            throw new InvalidAddressException("Country is required.");

        if (country.Length > AddressConstraints.CountryMaxLength)
            throw new InvalidAddressException(
                $"Country cannot exceed {AddressConstraints.CountryMaxLength} characters."
            );
    }

    private static void ValidateCustomerId(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new RequiredFieldException(nameof(customerId));
    }
}
