using System.Security.Claims;

namespace DeliveryService.Services.Models;

public sealed record AppUserDto(
    Guid UserId,
    string UserEmail,
    IList<string> Roles,
    IList<Claim> Claims
);

public sealed record RegisterUserDto(Guid UserId);

public sealed record LoginUserDto(string Email, TokenResponse Token);

public sealed record TokenResponse(string AccessToken, string RefreshToken, DateTime ExpiresOnUtc);
