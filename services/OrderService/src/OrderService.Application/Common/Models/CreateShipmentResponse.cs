namespace OrderService.Application.Common.Models;

public sealed record CreateShipmentResponse(Guid ShipmentId, Guid OrderId, string TrackingNumber);
