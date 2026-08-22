using FluentValidation;

namespace DeliveryService.Application.Features.Shipments.Commands.CreateShipment;

public sealed class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
{
    public CreateShipmentCommandValidator()
    {
        RuleFor(command => command.OrderId).NotEmpty();
        RuleFor(command => command.CustomerName).NotEmpty().MaximumLength(200);
        RuleFor(command => command.CustomerPhone).NotEmpty().MaximumLength(50);

        RuleFor(command => command.ShippingAddress).NotNull();
        RuleFor(command => command.ShippingAddress.AddressLine1).NotEmpty().MaximumLength(250);
        RuleFor(command => command.ShippingAddress.AddressLine2)
            .MaximumLength(250)
            .When(command => !string.IsNullOrWhiteSpace(command.ShippingAddress?.AddressLine2));
        RuleFor(command => command.ShippingAddress.City).NotEmpty().MaximumLength(100);
        RuleFor(command => command.ShippingAddress.State).NotEmpty().MaximumLength(100);
        RuleFor(command => command.ShippingAddress.Country).NotEmpty().MaximumLength(100);

        RuleFor(command => command.TotalAmount).GreaterThanOrEqualTo(0);
    }
}
