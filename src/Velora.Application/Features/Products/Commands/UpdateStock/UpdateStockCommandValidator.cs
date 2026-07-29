using FluentValidation;

namespace Velora.Application.Features.Products.Commands.UpdateStock;

public sealed class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
{
    public UpdateStockCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Quantity).GreaterThan(0);

        RuleFor(x => x.Operation).IsInEnum();
    }
}
