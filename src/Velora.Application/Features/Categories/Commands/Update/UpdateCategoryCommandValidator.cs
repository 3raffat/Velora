using FluentValidation;
using Velora.Application.Features.Categories.Commands.Update;

namespace Velora.Application.Features.Category.Commands.Update;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Name).MinimumLength(3).MaximumLength(20).NotEmpty();

        RuleFor(c => c.Description).MinimumLength(10).MaximumLength(500).NotEmpty();
    }
}
