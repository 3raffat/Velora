using FluentValidation;

namespace DeliveryService.Application.Features.Auth.Commands.Login;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(command => command.Email).NotEmpty().EmailAddress().MaximumLength(320);

        RuleFor(command => command.Password).NotEmpty().MinimumLength(8);
    }
}
