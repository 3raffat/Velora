using FluentValidation;

namespace OrderService.Application.Features.Orders.Commands.Confirm;

public sealed class ConfirmOrderCommandValidator : AbstractValidator<ConfirmOrderCommand>
{
    public ConfirmOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer id is required.");

        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order id is required.");
    }
}
