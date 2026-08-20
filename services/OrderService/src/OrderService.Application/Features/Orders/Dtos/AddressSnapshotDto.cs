namespace OrderService.Application.Features.Orders.Dtos;

public sealed record AddressSnapshotDto(
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string Country
);
