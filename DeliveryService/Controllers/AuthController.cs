using DeliveryService.Contracts;
using DeliveryService.Responces;
using DeliveryService.Services;
using DeliveryService.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers;

[ApiController]
[Tags("Auth")]
[Route("api/v1/auth")]
public sealed class AuthController(IUserService _userService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await _userService.LoginAsync(request.Email, request.Password, ct);
        return Ok(
            new StandardSuccessResponse<LoginUserDto>(
                result,
                StatusCodes.Status200OK,
                "Login Successful"
            )
        );
    }
}
