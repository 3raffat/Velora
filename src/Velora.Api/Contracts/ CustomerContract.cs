namespace Velora.Api.Contracts;

public sealed record CreateAddressRequest(
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string Country,
    Guid CustomerId
);

public sealed record UpdateAddressRequest(
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string Country,
    Guid CustomerId
);
