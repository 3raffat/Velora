using FluentValidation;

namespace OrderService.Application.Features.Products.Commands.UpdatePrice;

public sealed class UpdatePriceCommandValidator : AbstractValidator<UpdatePriceCommand>
{
    public UpdatePriceCommandValidator()
    {
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Id).NotEmpty().WithMessage("product is required.");
    }
}
