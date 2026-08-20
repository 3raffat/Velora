using FluentValidation;

namespace OrderService.Application.Features.ShoppingCarts.Commands.Checkout;

public sealed class CheckoutCartCommandValidator : AbstractValidator<CheckoutCartCommand>
{
    public CheckoutCartCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer id is required.");

        RuleFor(x => x.CartId).NotEmpty().WithMessage("Cart id is required.");

        RuleFor(x => x.BillingAddressId).NotEmpty().WithMessage("Billing address id is required.");

        RuleFor(x => x.ShippingAddressId)
            .NotEmpty()
            .WithMessage("Shipping address id is required.");

        RuleFor(x => x.PaymentMethod).IsInEnum().WithMessage("A valid payment method is required.");

        RuleFor(x => x.ShippingCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Shipping cost cannot be negative.");
    }
}
