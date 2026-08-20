using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Auth.Exceptions;

public sealed class EmailAlreadyConfirmedException : AuthException
{
    public EmailAlreadyConfirmedException()
        : base("Email is already confirmed.") { }
}
