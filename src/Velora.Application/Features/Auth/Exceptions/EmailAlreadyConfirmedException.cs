using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Auth.Exceptions;

public sealed class EmailAlreadyConfirmedException : AuthException
{
    public EmailAlreadyConfirmedException()
        : base("Email is already confirmed.") { }
}
