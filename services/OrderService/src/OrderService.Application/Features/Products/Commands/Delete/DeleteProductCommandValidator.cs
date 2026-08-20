using FluentValidation;

namespace OrderService.Application.Features.Products.Commands.Delete;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}
