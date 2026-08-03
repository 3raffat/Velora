using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Auth.Exceptions;

public sealed class InvalidEmailConfirmationTokenException : AuthException
{
    public InvalidEmailConfirmationTokenException()
        : base("The email confirmation token is invalid or has expired.") { }
}
