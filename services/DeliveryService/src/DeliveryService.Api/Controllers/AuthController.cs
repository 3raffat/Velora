using Asp.Versioning;
using DeliveryService.Api.Contracts.Auth;
using DeliveryService.Application.Common.Enums;
using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Common.Response;
using DeliveryService.Application.Features.Auth.Commands.Login;
using DeliveryService.Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:ApiVersion}/auth")]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    [Authorize(Roles = nameof(UserRole.DeliveryAdmin))]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<AuthResponse>),
        StatusCodes.Status201Created
    )]
    public async Task<ActionResult<StandardSuccessResponse<AuthResponse>>> Register(
        RegisterRequest request,
        CancellationToken ct
    )
    {
        var result = await sender.Send(
            new RegisterUserCommand(
                request.Username,
                request.Email,
                request.Password,
                request.Role
            ),
            ct
        );

        return StatusCode(
            StatusCodes.Status201Created,
            new StandardSuccessResponse<AuthResponse>(
                result,
                StatusCodes.Status201Created,
                "Registration successful."
            )
        );
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(StandardSuccessResponse<AuthResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<StandardSuccessResponse<AuthResponse>>> Login(
        LoginRequest request,
        CancellationToken ct
    )
    {
        var result = await sender.Send(new LoginUserCommand(request.Email, request.Password), ct);

        return Ok(
            new StandardSuccessResponse<AuthResponse>(
                result,
                StatusCodes.Status200OK,
                "Login successful."
            )
        );
    }
}
