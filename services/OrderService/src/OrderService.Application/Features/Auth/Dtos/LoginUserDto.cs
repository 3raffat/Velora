namespace OrderService.Application.Features.Auth.Dtos;

public sealed record LoginUserDto(string Email, IList<string> Roles, TokenResponse Token);
