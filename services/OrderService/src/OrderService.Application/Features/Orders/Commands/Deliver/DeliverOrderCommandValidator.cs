using FluentValidation;

namespace OrderService.Application.Features.Orders.Commands.Deliver;

public sealed class DeliverOrderCommandValidator : AbstractValidator<DeliverOrderCommand>
{
    public DeliverOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer id is required.");

        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order id is required.");
    }
}
