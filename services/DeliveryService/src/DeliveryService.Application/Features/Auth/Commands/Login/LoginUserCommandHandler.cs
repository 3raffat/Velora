using DeliveryService.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryService.Application.Features.Auth.Commands.Login;

public sealed class LoginUserCommandHandler(
    IUserService userService,
    ILogger<LoginUserCommandHandler> logger
) : IRequestHandler<LoginUserCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken ct)
    {
        var result = await userService.LoginAsync(request.Email, request.Password, ct);

        logger.LogInformation("User with email {Email} logged in successfully.", request.Email);
        return result;
    }
}
