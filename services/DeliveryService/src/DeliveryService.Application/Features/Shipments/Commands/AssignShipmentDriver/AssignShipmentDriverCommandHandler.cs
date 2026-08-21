using DeliveryService.Application.Common.Exceptions;
using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Application.Features.Shipments.Mappers;
using MediatR;

namespace DeliveryService.Application.Features.Shipments.Commands.AssignShipmentDriver;

public sealed class AssignShipmentDriverCommandHandler(IDeliveryContext context)
    : IRequestHandler<AssignShipmentDriverCommand, ShipmentDto>
{
    public async Task<ShipmentDto> Handle(AssignShipmentDriverCommand request, CancellationToken ct)
    {
        var shipment =
            await context.Shipments.FindAsync([request.ShipmentId], ct)
            ?? throw new NotFoundException("Shipment was not found.");

        shipment.AssignDriver(request.DriverId);

        await context.SaveChangesAsync(ct);

        return shipment.ToDto();
    }
}
