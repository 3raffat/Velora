using Velora.Application.Features.Addresses.Dtos;

namespace Velora.Application.Features.Customers.Dtos;

public record CustomerDto(
    Guid Id,
    Guid IdentityUserId,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber,
    DateOnly? DateOfBirth,
    bool IsProfileCompleted,
    IEnumerable<AddressDto>? Addresses
);
