using Velora.Domain.Common;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Customers.Errors;
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
    public string PhoneNumber { get; private set; } = string.Empty;
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
            throw new InvalidIdentityUserIdException();

        return new Customer(identityUserId, Guid.NewGuid());
    }

    public void CompleteProfile(
        Name firstName,
        Name lastName,
        Email email,
        string phoneNumber,
        DateOnly dateOfBirth
    )
    {
        if (IsProfileCompleted)
            throw new ProfileAlreadyCompletedException();

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required.", nameof(phoneNumber));

        if (dateOfBirth == default)
            throw new ArgumentException("Birth date is required.", nameof(dateOfBirth));

        if (dateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("Birth date cannot be in the future.", nameof(dateOfBirth));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber.Trim();
        DateOfBirth = dateOfBirth;
        IsProfileCompleted = true;
    }
}
