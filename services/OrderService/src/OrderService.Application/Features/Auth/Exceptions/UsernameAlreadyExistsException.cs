using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Auth.Exceptions;

public sealed class UsernameAlreadyExistsException : AuthException
{
    public UsernameAlreadyExistsException(string username)
        : base($"Username '{username}' already exists.") { }
}
