using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Auth.Exceptions;

public sealed class EmailAlreadyExistsException : AuthException
{
    public EmailAlreadyExistsException(string email)
        : base($"Email '{email}' is already registered.") { }
}
