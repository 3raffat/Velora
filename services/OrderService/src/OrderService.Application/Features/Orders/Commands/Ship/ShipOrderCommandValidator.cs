using FluentValidation;

namespace OrderService.Application.Features.Orders.Commands.Ship;

public sealed class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
{
    public ShipOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer id is required.");

        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order id is required.");
    }
}
