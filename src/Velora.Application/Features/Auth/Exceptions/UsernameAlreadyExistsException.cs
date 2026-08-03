using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Auth.Exceptions;

public sealed class UsernameAlreadyExistsException : AuthException
{
    public UsernameAlreadyExistsException(string username)
        : base($"Username '{username}' already exists.") { }
}
