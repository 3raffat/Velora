using DeliveryService.Application.Common.Exceptions;
using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Application.Features.Shipments.Mappers;
using DeliveryService.Domain.Entities.Shipments.Enums;
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

        if (shipment.DriverId != request.DriverId)
            throw new UnauthorizedException("Only the assigned driver can update this shipment.");

        shipment.ChangeStatus(request.Status, request.FailureReason);

        await context.SaveChangesAsync(ct);
        return shipment.ToDto();
    }
}
