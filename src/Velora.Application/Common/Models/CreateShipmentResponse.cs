namespace Velora.Application.Common.Models;

public sealed record CreateShipmentResponse(Guid ShipmentId, Guid OrderId, string TrackingNumber);
