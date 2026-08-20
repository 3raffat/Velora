using FluentValidation;

namespace OrderService.Application.Features.Products.Commands.Update;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MinimumLength(3)
            .MaximumLength(20);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500);

        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .WithMessage("Image URL is required.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Image URL must be a valid absolute URL.");

        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required.");
    }
}
