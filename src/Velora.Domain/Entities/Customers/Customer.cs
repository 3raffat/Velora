using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Customers.Exceptions;
using Velora.Domain.Entities.Customers.ValueObjects;
using Velora.Domain.Entities.Orders;
using Velora.Domain.Entities.Products;
using Velora.Domain.Entities.ShoppingCart;

namespace Velora.Domain.Entities.Customers;

public sealed class Customer : BaseEntity
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
    }
}
