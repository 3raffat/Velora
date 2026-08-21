using DeliveryService.Application.Common.Exceptions;
using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Application.Features.Shipments.Mappers;
using MediatR;

namespace DeliveryService.Application.Features.Shipments.Commands.UpdateShipmentStatus;

public sealed class UpdateShipmentStatusCommandHandler(IDeliveryContext context)
    : IRequestHandler<UpdateShipmentStatusCommand, ShipmentDto>
{
    public async Task<ShipmentDto> Handle(UpdateShipmentStatusCommand request, CancellationToken ct)
    {
        var shipment =
            await context.Shipments.FindAsync([request.ShipmentId], ct)
            ?? throw new NotFoundException("Shipment was not found.");

        shipment.ChangeStatus(request.Status, request.FailureReason);

        await context.SaveChangesAsync(ct);
        return shipment.ToDto();
    }
}
