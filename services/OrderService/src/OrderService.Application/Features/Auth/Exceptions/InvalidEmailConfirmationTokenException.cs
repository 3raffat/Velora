using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Auth.Exceptions;

public sealed class InvalidEmailConfirmationTokenException : AuthException
{
    public InvalidEmailConfirmationTokenException()
        : base("The email confirmation token is invalid or has expired.") { }
}
