using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Auth.Dtos;
using Velora.Application.Features.Auth.Exceptions;
using Velora.Infrastructure.Services.Models;

namespace Velora.Infrastructure.Services;

public sealed class UserService(
    ILogger<UserService> _logger,
    UserManager<AppUser> _manager,
    ITokenProvider _token,
    IEmailService _emailService
) : IUserService
{
    public async Task ConfirmEmailAsync(string userId, string token, CancellationToken ct)
    {
        var user = await GetUserByIdsAsync(userId);

        if (user is null)
            throw new UserNotFoundException(userId);

        if (user.EmailConfirmed)
            throw new EmailAlreadyConfirmedException();

        var decodedToken = DecodedToken(token);

        var result = await _manager.ConfirmEmailAsync(user, decodedToken);

        if (!result.Succeeded)
        {
            throw new InvalidEmailConfirmationTokenException();
        }
    }

    public async Task<LoginUserDto> LoginAsync(string email, string password, CancellationToken ct)
    {
        var user = await _manager.FindByEmailAsync(email);

        if (user is null)
            throw new InvalidCredentialsException();

        if (!user.EmailConfirmed)
            throw new EmailNotConfirmedException();

        var validPassword = await _manager.CheckPasswordAsync(user, password);

        if (!validPassword)
            throw new InvalidCredentialsException();

        var userInfo = await GetUserInfoAsync(user);

        var tokenResult = await _token.GenerateJwtTokenAsync(userInfo, ct);

        return new LoginUserDto(email, tokenResult);
    }

    public async Task RegisterAsync(
        string username,
        string email,
        string password,
        CancellationToken ct
    )
    {
        var existingUsername = await _manager.FindByNameAsync(username);

        if (existingUsername is not null)
            throw new UsernameAlreadyExistsException(username);

        var existingEmail = await _manager.FindByEmailAsync(email);

        if (existingEmail is not null)
        {
            _logger.LogWarning(
                "Registration attempt failed: Email {Email} is already registered.",
                email
            );
            throw new EmailAlreadyExistsException(email);
        }

        var newUser = AppUser.Create(username, email);

        var createResult = await _manager.CreateAsync(newUser, password);

        if (!createResult.Succeeded)
            throw new InvalidOperationException(
                "User creation failed. Please check the provided details and try again."
            );

        var addToRoleResult = await _manager.AddToRoleAsync(newUser, "User");

        if (!addToRoleResult.Succeeded)
        {
            await _manager.DeleteAsync(newUser);
            throw new InvalidOperationException("Failed to assign role to user. Please try again.");
        }

        _logger.LogInformation("User with email {Email} successfully registered.", email);

        var token = await _manager.GenerateEmailConfirmationTokenAsync(newUser);

        var encodedToken = EncodedToken(token);

        var confirmationLink =
            $"https://localhost:5000/auth/v1/confirm-email?userId={newUser.Id}&token={encodedToken}";

        // await _emailService.SendConfirmationEmailAsync(email, username, confirmationLink, ct);
    }

    private async Task<AppUserDto> GetUserInfoAsync(AppUser user)
    {
        var roles = await _manager.GetRolesAsync(user);

        var claims = await _manager.GetClaimsAsync(user);

        return new AppUserDto(user.GetIdString(), user.Email!, roles, claims);
    }

    public async Task<AppUserDto?> GetUserByIdAsync(string userId)
    {
        var user =
            await _manager.FindByIdAsync(userId)
            ?? throw new InvalidOperationException(nameof(userId));

        var roles = await _manager.GetRolesAsync(user);

        var claims = await _manager.GetClaimsAsync(user);

        return new AppUserDto(user.Id.ToString(), user.Email!, roles, claims);
    }

    private async Task<AppUser?> GetUserByIdsAsync(string userId)
    {
        return await _manager.FindByIdAsync(userId);
    }

    private string DecodedToken(string token)
    {
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

        return decodedToken;
    }

    private string EncodedToken(string token)
    {
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        return encodedToken;
    }
}
