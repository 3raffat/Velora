using Asp.Versioning;
using DeliveryService.Api.Contracts.Auth;
using DeliveryService.Application.Common.Enums;
using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Common.Response;
using DeliveryService.Application.Features.Auth.Commands.Login;
using DeliveryService.Application.Features.Auth.Commands.Register;
using DeliveryService.Application.Features.Users.Commands.SetUserActive;
using DeliveryService.Application.Features.Users.Queries.GetUsers;
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
    [EndpointSummary("Register a user")]
    [EndpointDescription("Registers a new user account.")]
    [EndpointName("RegisterUser")]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<AuthResponse>),
        StatusCodes.Status201Created
    )]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken ct)
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
    [EndpointSummary("Log in")]
    [EndpointDescription("Authenticates a user and returns an access token.")]
    [EndpointName("LoginUser")]
    [ProducesResponseType(typeof(StandardSuccessResponse<AuthResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
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

    [HttpGet("users")]
    [Authorize(Roles = nameof(UserRole.DeliveryAdmin) + "," + nameof(UserRole.Dispatcher))]
    [EndpointSummary("Get all users")]
    [EndpointDescription(
        "Retrieves registered users, optionally filtered by role, and their active status."
    )]
    [EndpointName("GetAllUsers")]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<IReadOnlyCollection<UserSummary>>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetUsers([FromQuery] UserRole? role, CancellationToken ct)
    {
        var result = await sender.Send(new GetUsersQuery(role), ct);

        return Ok(
            new StandardSuccessResponse<IReadOnlyCollection<UserSummary>>(
                result,
                StatusCodes.Status200OK,
                "Users retrieved successfully."
            )
        );
    }

    [HttpPatch("users/{userId:guid}/activate")]
    [Authorize(Roles = nameof(UserRole.DeliveryAdmin))]
    [EndpointSummary("Activate a user")]
    [EndpointDescription("Activates a user account.")]
    [EndpointName("ActivateUser")]
    [ProducesResponseType(typeof(StandardSuccessResponse<UserSummary>), StatusCodes.Status200OK)]
    public Task<IActionResult> ActivateUser(Guid userId, CancellationToken ct) =>
        SetUserActive(userId, true, ct);

    [HttpPatch("users/{userId:guid}/deactivate")]
    [Authorize(Roles = nameof(UserRole.DeliveryAdmin))]
    [EndpointSummary("Deactivate a user")]
    [EndpointDescription("Deactivates a user account and prevents login.")]
    [EndpointName("DeactivateUser")]
    [ProducesResponseType(typeof(StandardSuccessResponse<UserSummary>), StatusCodes.Status200OK)]
    public Task<IActionResult> DeactivateUser(Guid userId, CancellationToken ct) =>
        SetUserActive(userId, false, ct);

    private async Task<IActionResult> SetUserActive(
        Guid userId,
        bool isActive,
        CancellationToken ct
    )
    {
        var result = await sender.Send(new SetUserActiveCommand(userId, isActive), ct);

        return Ok(
            new StandardSuccessResponse<UserSummary>(
                result,
                StatusCodes.Status200OK,
                isActive ? "User activated successfully." : "User deactivated successfully."
            )
        );
    }
}
