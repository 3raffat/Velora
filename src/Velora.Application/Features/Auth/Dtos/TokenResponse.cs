namespace Velora.Application.Features.Auth.Dtos;

public sealed record TokenResponse(string AccessToken, string RefreshToken, DateTime ExpiresOnUtc);
