using FluentValidation;

namespace Velora.Application.Features.Orders.Commands.Cancel;

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer id is required.");

        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order id is required.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Cancellation reason is required.")
            .MaximumLength(500)
            .WithMessage("Cancellation reason cannot exceed 500 characters.");
    }
}
