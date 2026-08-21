using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Application.Features.Shipments.Mappers;
using DeliveryService.Domain.Common.ValueObjects;
using DeliveryService.Domain.Entities.Shipments;
using MediatR;

namespace DeliveryService.Application.Features.Shipments.Commands.CreateShipment;

public sealed class CreateShipmentCommandHandler(IDeliveryContext context)
    : IRequestHandler<CreateShipmentCommand, CreateShipmentDto>
{
    public async Task<CreateShipmentDto> Handle(CreateShipmentCommand request, CancellationToken ct)
    {
        var shipment = Shipment.Create(
            request.OrderId,
            request.RecipientName,
            request.RecipientPhone,
            AddressSnapshot.Create(
                request.AddressLine1,
                request.AddressLine2,
                request.City,
                request.State,
                request.Country
            ),
            request.TotalAmount
        );

        context.Shipments.Add(shipment);
        await context.SaveChangesAsync(ct);
        return shipment.ToCreateDto();
    }
}
