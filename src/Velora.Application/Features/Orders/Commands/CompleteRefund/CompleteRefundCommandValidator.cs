using FluentValidation;

namespace Velora.Application.Features.Orders.Commands.CompleteRefund;

public sealed class CompleteRefundCommandValidator : AbstractValidator<CompleteRefundCommand>
{
    public CompleteRefundCommandValidator()
    {
        RuleFor(x => x.ProcessedBy).NotEmpty().WithMessage("ProcessedBy is required.");

        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order id is required.");

        RuleFor(x => x.TransactionId)
            .NotEmpty()
            .WithMessage("Transaction id is required.")
            .MaximumLength(200)
            .WithMessage("Transaction id cannot exceed 200 characters.");
    }
}
