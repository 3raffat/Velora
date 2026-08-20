using FluentValidation;

namespace OrderService.Application.Features.Orders.Commands.RequestCancellation;

public sealed class RequestCancellationCommandValidator
    : AbstractValidator<RequestCancellationCommand>
{
    public RequestCancellationCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order ID is required.");

        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Cancellation reason is required.")
            .MaximumLength(500)
            .WithMessage("Cancellation reason cannot exceed 500 characters.");
    }
}
