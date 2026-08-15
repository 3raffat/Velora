using FluentValidation;
using MediatR;

namespace Velora.Application.Features.Orders.Commands.RejectRefund;

public sealed class RejectRefundCommandValidator : AbstractValidator<RejectRefundCommand>
{
    public RejectRefundCommandValidator()
    {
        RuleFor(x => x.ProcessedBy).NotEmpty().WithMessage("ProcessedBy is required.");

        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order id is required.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Rejection reason is required.")
            .MaximumLength(1000)
            .WithMessage("Rejection reason cannot exceed 1000 characters.");
    }
}
