using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Common.Response;
using Velora.Application.Features.Auth.Commands.ConfirmEmail;
using Velora.Application.Features.Auth.Commands.Login;
using Velora.Application.Features.Auth.Commands.Register;
using Velora.Application.Features.Auth.Dtos;

namespace Velora.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:ApiVersion}/auth")]
public sealed class AuthController(ISender _sender) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CustomerLoginCommand(request.Email, request.Password),
            ct
        );
        return Ok(
            new StandardSuccessResponse<LoginUserDto>(
                result,
                StatusCodes.Status200OK,
                "Login Successful"
            )
        );
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterRequest request, CancellationToken ct)
    {
        await _sender.Send(
            new CustomerRegisterCommand(request.UserName, request.Email, request.Password),
            ct
        );

        return StatusCode(
            StatusCodes.Status201Created,
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status201Created,
                "Registration Successful, Please check your email to confirm your account."
            )
        );
    }

    [HttpGet("confirm-email/{userId:guid}")]
    public async Task<IActionResult> ConfirmEmail(
        [FromRoute] string userId,
        [FromQuery] string token,
        CancellationToken ct
    )
    {
        await _sender.Send(new ConfirmEmailCommand(userId, token), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Email confirmed successfully. You can now log in to your account."
            )
        );
    }
}
