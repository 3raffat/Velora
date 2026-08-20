using FluentValidation;

namespace OrderService.Application.Features.Orders.Commands.ApproveRefund;

public sealed class ApproveRefundCommandValidator : AbstractValidator<ApproveRefundCommand>
{
    public ApproveRefundCommandValidator()
    {
        RuleFor(x => x.ProcessedBy).NotEmpty().WithMessage("ProcessedBy is required.");

        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order id is required.");
    }
}
