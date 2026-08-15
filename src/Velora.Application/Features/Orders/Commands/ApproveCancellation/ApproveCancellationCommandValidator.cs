using FluentValidation;

namespace Velora.Application.Features.Orders.Commands.ApproveCancellation;

public sealed class ApproveCancellationCommandValidator
    : AbstractValidator<ApproveCancellationCommand>
{
    public ApproveCancellationCommandValidator()
    {
        RuleFor(x => x.ProcessedBy).NotEmpty().WithMessage("ProcessedBy is required.");

        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order id is required.");

        RuleFor(x => x.CancellationCharges)
            .GreaterThanOrEqualTo(0)
            .When(x => x.CancellationCharges.HasValue)
            .WithMessage("Cancellation charges cannot be negative.");
    }
}
