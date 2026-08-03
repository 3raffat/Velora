using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Auth.Exceptions;

public sealed class UserNotFoundException : AuthException
{
    public UserNotFoundException(Guid userId)
        : base($"User with Id {userId} not found.") { }

    public UserNotFoundException(string userId)
        : base($"User with Id {userId} not found.") { }
}
