namespace OrderService.Application.Common.Models;

public sealed record CreateShipmentResponse(Guid OrderId, string TrackingNumber);
