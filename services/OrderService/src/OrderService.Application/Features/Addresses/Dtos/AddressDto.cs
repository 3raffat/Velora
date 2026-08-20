namespace OrderService.Application.Features.Addresses.Dtos;

public record AddressDto(
    Guid Id,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string Country,
    Guid CustomerId
);
