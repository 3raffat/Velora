using System.Security.Claims;
using OrderService.Application.Features.Auth.Dtos;

namespace OrderService.Application.Common.Interfaces;

public interface ITokenProvider
{
    Task<TokenResponse> GenerateJwtTokenAsync(
        Guid CustomerId,
        AppUserDto user,
        CancellationToken ct = default
    );
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
