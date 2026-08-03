using System.Security.Claims;
using Velora.Application.Features.Auth.Dtos;

namespace Velora.Application.Common.Interfaces;

public interface ITokenProvider
{
    Task<TokenResponse> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct = default);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
