using FluentValidation;

namespace Velora.Application.Features.Addresses.Commands.Update;

public sealed class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressCommandValidator()
    {
        RuleFor(x => x.AddressLine1).NotEmpty().MaximumLength(200);

        RuleFor(x => x.AddressLine2)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.AddressLine2));

        RuleFor(x => x.City).NotEmpty().MaximumLength(100);

        RuleFor(x => x.State).NotEmpty().MaximumLength(100);

        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);

        RuleFor(x => x.CustomerId).NotEmpty();

        RuleFor(x => x.AddressId).NotEmpty();
    }
}
