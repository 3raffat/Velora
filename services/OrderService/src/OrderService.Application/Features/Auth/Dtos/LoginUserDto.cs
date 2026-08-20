namespace OrderService.Application.Features.Auth.Dtos;

public sealed record LoginUserDto(string Email, TokenResponse Token);
