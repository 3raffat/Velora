using FluentValidation;

namespace Velora.Application.Features.Addresses.Commands.Delete;

public sealed class DeleteAddressCommandValidator : AbstractValidator<DeleteAddressCommand>
{
    public DeleteAddressCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();

        RuleFor(x => x.AddressId).NotEmpty();
    }
}
