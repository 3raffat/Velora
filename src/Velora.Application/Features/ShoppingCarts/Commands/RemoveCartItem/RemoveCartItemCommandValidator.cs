using FluentValidation;

namespace Velora.Application.Features.ShoppingCarts.Commands.RemoveCartItem;

public sealed class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
{
    public RemoveCartItemCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer id is required.");

        RuleFor(x => x.CartId).NotEmpty().WithMessage("Cart id is required.");

        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product id is required.");
    }
}
