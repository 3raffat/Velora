using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Auth.Exceptions;

public sealed class EmailNotConfirmedException : AuthException
{
    public EmailNotConfirmedException()
        : base("Email has not been confirmed.") { }
}
