namespace DeliveryService.Application.Features.Shipments.Dtos;

public sealed record CreateShipmentDto(Guid OrderId, string TrackingNumber);
