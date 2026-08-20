using FluentValidation;
using OrderService.Application.Features.Categories.Commands.Create;

namespace OrderService.Application.Features.Category.Commands.Create;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name).MinimumLength(3).MaximumLength(100).NotEmpty();

        RuleFor(c => c.Description).MinimumLength(10).MaximumLength(500).NotEmpty();
    }
}
