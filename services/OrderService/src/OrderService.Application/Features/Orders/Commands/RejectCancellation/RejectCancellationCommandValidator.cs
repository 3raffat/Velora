using FluentValidation;

namespace OrderService.Application.Features.Orders.Commands.RejectCancellation;

public sealed class RejectCancellationCommandValidator
    : AbstractValidator<RejectCancellationCommand>
{
    public RejectCancellationCommandValidator()
    {
        RuleFor(x => x.ProcessedBy).NotEmpty().WithMessage("ProcessedBy is required.");

        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order id is required.");

        RuleFor(x => x.Remarks)
            .NotEmpty()
            .WithMessage("Remarks are required when rejecting a cancellation.")
            .MaximumLength(1000)
            .WithMessage("Remarks cannot exceed 1000 characters.");
    }
}
