using OrderService.Application.Features.Auth.Dtos;

namespace OrderService.Application.Common.Interfaces;

public interface IUserService
{
    Task<LoginUserDto> LoginAsync(string email, string password, CancellationToken ct);
    Task<RegisterUserDto> RegisterAsync(
        string username,
        string email,
        string password,
        CancellationToken ct
    );
    Task ConfirmEmailAsync(string userId, string token, CancellationToken ct);
    Task<AppUserDto?> GetUserByIdAsync(string userId);
}
