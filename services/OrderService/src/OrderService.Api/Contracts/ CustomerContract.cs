namespace OrderService.Api.Contracts;

public sealed record CreateAddressRequest(
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string Country
);

public sealed record UpdateAddressRequest(
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string Country
);

public sealed record CompleteCustomerProfileRequest(
    Guid IdentityId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    DateOnly DateOfBirth
);
