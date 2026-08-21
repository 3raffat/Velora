using FluentValidation;

namespace DeliveryService.Application.Features.Shipments.Commands.AssignShipmentDriver;

public sealed class AssignShipmentDriverCommandValidator
    : AbstractValidator<AssignShipmentDriverCommand>
{
    public AssignShipmentDriverCommandValidator()
    {
        RuleFor(command => command.ShipmentId).NotEmpty();
        RuleFor(command => command.DriverId).NotEmpty();
    }
}
