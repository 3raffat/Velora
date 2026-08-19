using System.Text;
using DeliveryService.Data;
using DeliveryService.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace DeliveryService.Services;

public interface IUserService
{
    Task<LoginUserDto> LoginAsync(string email, string password, CancellationToken ct);

    Task<RegisterUserDto> RegisterAsync(
        string username,
        string email,
        string password,
        CancellationToken ct
    );

    Task<AppUserDto?> GetUserByIdAsync(string userId);
}

class UserService(
    ILogger<UserService> _logger,
    UserManager<AppUser> _manager,
    ITokenProvider _token
) : IUserService
{
    public async Task<LoginUserDto> LoginAsync(string email, string password, CancellationToken ct)
    {
        var user = await _manager.FindByEmailAsync(email);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.EmailConfirmed)
            throw new UnauthorizedAccessException("Email is not confirmed.");

        var validPassword = await _manager.CheckPasswordAsync(user, password);

        if (!validPassword)
            throw new UnauthorizedAccessException("Invalid email or password.");

        var userInfo = await GetUserInfoAsync(user);

        var tokenResult = await _token.GenerateJwtTokenAsync(user.Id, userInfo, ct);

        return new LoginUserDto(email, tokenResult);
    }

    public async Task<RegisterUserDto> RegisterAsync(
        string username,
        string email,
        string password,
        CancellationToken ct
    )
    {
        var existingUsername = await _manager.FindByNameAsync(username);

        if (existingUsername is not null)
            throw new ArgumentException($"Username '{username}' already exists.");

        var existingEmail = await _manager.FindByEmailAsync(email);

        if (existingEmail is not null)
            throw new ArgumentException($"Email '{email}' already exists.");

        var newUser = AppUser.Create(username, email);

        var createResult = await _manager.CreateAsync(newUser, password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));

            throw new InvalidOperationException(errors);
        }

        _logger.LogInformation("User with email {Email} successfully registered.", email);

        return new RegisterUserDto(newUser.Id);
    }

    public async Task<AppUserDto?> GetUserByIdAsync(string userId)
    {
        var user =
            await _manager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID '{userId}' was not found.");

        return await GetUserInfoAsync(user);
    }

    private async Task<AppUserDto> GetUserInfoAsync(AppUser user)
    {
        var roles = await _manager.GetRolesAsync(user);
        var claims = await _manager.GetClaimsAsync(user);

        return new AppUserDto(user.Id, user.Email!, roles, claims);
    }
}
