using MediatR;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;

namespace Velora.Application.Features.Auth.Commands.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler(
    IUserService _userService,
    ILogger<ConfirmEmailCommandHandler> _logger
) : IRequestHandler<ConfirmEmailCommand>
{
    public async Task Handle(ConfirmEmailCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Confirming email for user {UserId}", request.UserId);

        await _userService.ConfirmEmailAsync(request.UserId, request.Token, ct);

        _logger.LogInformation("Email confirmed for user {UserId}", request.UserId);
    }
}
