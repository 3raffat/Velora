using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Auth.Exceptions;

public sealed class EmailAlreadyExistsException : AuthException
{
    public EmailAlreadyExistsException(string email)
        : base($"Email '{email}' is already registered.") { }
}
