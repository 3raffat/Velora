using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Customers.Events;
using Velora.Domain.Entities.Customers.Exceptions;
using Velora.Domain.Entities.Customers.ValueObjects;
using Velora.Domain.Entities.Orders;
using Velora.Domain.Entities.Products;
using Velora.Domain.Entities.ShoppingCart;

namespace Velora.Domain.Entities.Customers;

public sealed class Customer : SoftDeletableEntity
{
    public Guid IdentityUserId { get; private set; }
    public Name FirstName { get; private set; } = null!;
    public Name LastName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public DateOnly DateOfBirth { get; private set; }
    public bool IsProfileCompleted { get; private set; }

    private readonly List<Order> _orders = new();
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    private readonly List<Address> _addresses = new();
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    private readonly List<Cart> _carts = new();
    public IReadOnlyCollection<Cart> Carts => _carts.AsReadOnly();

    private readonly List<Feedback> _feedbacks = new();

    public IReadOnlyCollection<Feedback> Feedbacks => _feedbacks.AsReadOnly();

    private const int MaxAddresses = 5;

    private Customer(Guid identityUserId, Guid id)
        : base(id)
    {
        IdentityUserId = identityUserId;
    }

    public static Customer Create(Guid identityUserId)
    {
        if (Guid.Empty == identityUserId)
            throw new RequiredFieldException(nameof(identityUserId));

        return new Customer(identityUserId, Guid.NewGuid());
    }

    public void CompleteProfile(
        Name firstName,
        Name lastName,
        Email email,
        PhoneNumber phoneNumber,
        DateOnly dateOfBirth
    )
    {
        if (IsProfileCompleted)
            throw new ProfileAlreadyCompletedException();

        if (dateOfBirth == default)
            throw new InvalidDateOfBirthException();

        if (dateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new InvalidDateOfBirthException(dateOfBirth);

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
        IsProfileCompleted = true;

        AddDomainEvent(new CustomerProfileCompletedEvent(Id));
    }

    public void AddAddress(
        string addressLine1,
        string addressLine2,
        string city,
        string state,
        string country
    )
    {
        if (_addresses.Count >= MaxAddresses)
            throw new MaxAddressesReachedException(MaxAddresses);

        var address = Address.Create(addressLine1, addressLine2, city, state, country, Id);

        _addresses.Add(address);
    }

    public void UpdateAddress(
        Guid addressId,
        string addressLine1,
        string addressLine2,
        string city,
        string state,
        string country
    )
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);

        if (address is null)
            throw new AddressNotFoundException(addressId);

        address.Update(addressLine1, addressLine2, city, state, country);
    }

    public void RemoveAddress(Guid addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);

        if (address is null)
            throw new AddressNotFoundException(addressId);

        _addresses.Remove(address);
    }
}
