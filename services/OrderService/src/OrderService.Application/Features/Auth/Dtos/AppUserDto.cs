using System.Security.Claims;

namespace OrderService.Application.Features.Auth.Dtos;

public sealed record AppUserDto(
    Guid UserId,
    string UserEmail,
    IList<string> Roles,
    IList<Claim> Claims
);

public sealed record UserLoginRequest(string Email, string Password);

public sealed record UserRegisterRequest(string UserName, string Email, string Password);
