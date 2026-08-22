using FluentValidation;

namespace OrderService.Application.Features.Orders.Commands.Ship;

public sealed class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
{
    public ShipOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order id is required.");
    }
}
