namespace DeliveryService.Api.Contracts.Shipments;

public sealed record CreateShipmentRequest(
    Guid OrderId,
    string RecipientName,
    string RecipientPhone,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string Country,
    decimal TotalAmount
);
