using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Auth.Exceptions;

public sealed class InvalidCredentialsException : AuthException
{
    public InvalidCredentialsException()
        : base("Invalid email or password.") { }
}
