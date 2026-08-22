using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OrderService.Application.Common.Response;
using OrderService.Application.Features.Auth.Commands.ConfirmEmail;
using OrderService.Application.Features.Auth.Commands.Login;
using OrderService.Application.Features.Auth.Commands.Register;
using OrderService.Application.Features.Auth.Dtos;

namespace OrderService.Api.Controllers;

[ApiController]
[Tags("Auth")]
[ApiVersion(1)]
[Route("api/v{version:ApiVersion}/auth")]
public sealed class AuthController(ISender _sender) : ControllerBase
{
    [HttpPost("login")]
    [EndpointName("Login")]
    [EndpointSummary("Log in")]
    [EndpointDescription("Authenticates a customer and returns an access token.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<LoginUserDto>), StatusCodes.Status200OK)]
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
    [EndpointName("Register")]
    [EndpointSummary("Register customer")]
    [EndpointDescription("Creates a customer account and sends an email confirmation request.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status201Created)]
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
    [EndpointName("ConfirmEmail")]
    [EndpointSummary("Confirm email")]
    [EndpointDescription("Confirms a customer email address using the supplied token.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
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
