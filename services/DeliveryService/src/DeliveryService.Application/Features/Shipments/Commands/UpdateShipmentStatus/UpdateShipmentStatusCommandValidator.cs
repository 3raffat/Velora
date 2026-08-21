using DeliveryService.Domain.Entities.Shipments.Enums;
using FluentValidation;

namespace DeliveryService.Application.Features.Shipments.Commands.UpdateShipmentStatus;

public sealed class UpdateShipmentStatusCommandValidator
    : AbstractValidator<UpdateShipmentStatusCommand>
{
    public UpdateShipmentStatusCommandValidator()
    {
        RuleFor(command => command.ShipmentId).NotEmpty();
        RuleFor(command => command.Status).IsInEnum();
        RuleFor(command => command.FailureReason)
            .NotEmpty()
            .MaximumLength(1000)
            .When(command => command.Status == ShipmentStatus.Failed);
    }
}
