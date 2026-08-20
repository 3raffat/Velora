using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Auth.Exceptions;

public sealed class EmailNotConfirmedException : AuthException
{
    public EmailNotConfirmedException()
        : base("Email has not been confirmed.") { }
}
