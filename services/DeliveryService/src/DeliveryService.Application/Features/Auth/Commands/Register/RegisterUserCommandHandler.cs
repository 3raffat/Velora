using DeliveryService.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryService.Application.Features.Auth.Commands.Register;

public sealed class RegisterUserCommandHandler(
    IUserService userService,
    ILogger<RegisterUserCommandHandler> logger
) : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        var result = await userService.RegisterAsync(
            request.Username,
            request.Email,
            request.Password,
            request.Role,
            ct
        );

        logger.LogInformation("User with email {Email} registered successfully.", request.Email);
        return result;
    }
}
