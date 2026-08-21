using FluentValidation;

namespace DeliveryService.Application.Features.Shipments.Commands.CreateShipment;

public sealed class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
{
    public CreateShipmentCommandValidator()
    {
        RuleFor(command => command.OrderId).NotEmpty();
        RuleFor(command => command.RecipientName).NotEmpty().MaximumLength(200);
        RuleFor(command => command.RecipientPhone).NotEmpty().MaximumLength(50);
        RuleFor(command => command.AddressLine1).NotEmpty().MaximumLength(250);
        RuleFor(command => command.AddressLine2).MaximumLength(250);
        RuleFor(command => command.City).NotEmpty().MaximumLength(100);
        RuleFor(command => command.State).NotEmpty().MaximumLength(100);
        RuleFor(command => command.Country).NotEmpty().MaximumLength(100);
        RuleFor(command => command.TotalAmount).GreaterThanOrEqualTo(0);
    }
}
