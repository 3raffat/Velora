using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Common.Response;
using Velora.Application.Features.Auth.Commands.Login;
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
}
