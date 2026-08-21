using System.Security.Claims;

namespace DeliveryService.Application.Common.Interfaces;

public interface ITokenProvider
{
    TokenResult GenerateToken(UserTokenData user);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}

public sealed record UserTokenData(Guid Id, string? Email, IList<string> Roles);

public sealed record TokenResult(string AccessToken, DateTime ExpiresAt);
