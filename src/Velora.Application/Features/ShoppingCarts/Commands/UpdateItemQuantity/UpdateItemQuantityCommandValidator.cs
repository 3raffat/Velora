using FluentValidation;

namespace Velora.Application.Features.ShoppingCarts.Commands.UpdateItemQuantity;

public sealed class UpdateItemQuantityCommandValidator
    : AbstractValidator<UpdateItemQuantityCommand>
{
    public UpdateItemQuantityCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer id is required.");

        RuleFor(x => x.CartId).NotEmpty().WithMessage("Cart id is required.");

        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product id is required.");

        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}
