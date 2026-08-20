using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Auth.Exceptions;

public sealed class InvalidCredentialsException : AuthException
{
    public InvalidCredentialsException()
        : base("Invalid email or password.") { }
}
