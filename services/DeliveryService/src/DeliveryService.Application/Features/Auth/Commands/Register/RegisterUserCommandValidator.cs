using DeliveryService.Application.Common.Enums;
using FluentValidation;

namespace DeliveryService.Application.Features.Auth.Commands.Register;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Username).NotEmpty().MinimumLength(3).MaximumLength(100);

        RuleFor(command => command.Email).NotEmpty().EmailAddress().MaximumLength(320);

        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]")
            .Matches("[a-z]")
            .Matches("[0-9]")
            .Matches("[^a-zA-Z0-9]");

        RuleFor(command => command.Role)
            .Must(role => Enum.IsDefined(typeof(UserRole), role))
            .WithMessage("A valid user role is required.")
            .Must(role => role is UserRole.User or UserRole.Driver)
            .WithMessage("Only User and Driver accounts can self-register.");
    }
}
