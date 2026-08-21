using DeliveryService.Application.Features.DeliveryAttempts.Mappers;
using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Domain.Entities.Shipments;

namespace DeliveryService.Application.Features.Shipments.Mappers;

public static class ShipmentMapper
{
    public static CreateShipmentDto ToCreateDto(this Shipment shipment) =>
        new(shipment.OrderId, shipment.TrackingNumber.Value);

    public static IReadOnlyCollection<ShipmentDto> ToDto(this IEnumerable<Shipment> shipments) =>
        shipments.Select(shipment => shipment.ToDto()).ToArray();

    public static ShipmentDto ToDto(this Shipment shipment) =>
        new(
            shipment.Id,
            shipment.OrderId,
            shipment.TrackingNumber.Value,
            shipment.RecipientName,
            shipment.RecipientPhone,
            shipment.DeliveryAddress.AddressLine1,
            shipment.DeliveryAddress.AddressLine2,
            shipment.DeliveryAddress.City,
            shipment.DeliveryAddress.State,
            shipment.DeliveryAddress.Country,
            shipment.TotalAmount,
            shipment.Status.ToString(),
            shipment.DriverId,
            shipment.PickedUpAt,
            shipment.DeliveredAt,
            shipment.FailureReason,
            shipment
                .DeliveryAttempts.OrderByDescending(attempt => attempt.AttemptedAt)
                .Select(attempt => attempt.ToDto())
                .ToArray()
        );
}
