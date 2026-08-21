using DeliveryService.Application.Common.Enums;
using DeliveryService.Application.Common.Exceptions;
using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Infrastructure.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace DeliveryService.Infrastructure.Services;

public sealed class UserService(UserManager<AppUser> userManager, ITokenProvider tokenProvider)
    : IUserService
{
    public async Task<AuthResponse> RegisterAsync(
        string username,
        string email,
        string password,
        UserRole role,
        CancellationToken ct = default
    )
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidRequestException("Username is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidRequestException("Email is required.");

        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidRequestException("Password is required.");

        if (!Enum.IsDefined(typeof(UserRole), role))
            throw new InvalidRequestException("The selected role is invalid.");

        if (role is UserRole.Dispatcher or UserRole.DeliveryAdmin)
            throw new UnauthorizedException("Privileged accounts cannot be self-registered.");

        if (await userManager.FindByNameAsync(username) is not null)
            throw new ConflictException("Username is already registered.");

        if (await userManager.FindByEmailAsync(email) is not null)
            throw new ConflictException("Email is already registered.");

        var user = AppUser.Create(username, email);
        var result = await userManager.CreateAsync(user, password);
        EnsureSucceeded(result);

        var roleResult = await userManager.AddToRoleAsync(user, role.ToString());
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            EnsureSucceeded(roleResult);
        }

        return await CreateAuthResponseAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(
        string email,
        string password,
        CancellationToken ct = default
    )
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null || !await userManager.CheckPasswordAsync(user, password))
            throw new UnauthorizedException("Invalid email or password.");

        if (!user.EmailConfirmed)
            throw new UnauthorizedException("Email address is not confirmed.");

        return await CreateAuthResponseAsync(user);
    }

    public async Task<bool> IsDriverAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        return user is not null && await userManager.IsInRoleAsync(user, nameof(UserRole.Driver));
    }

    public async Task<UserSummary?> GetUserByIdAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return null;

        var roles = await userManager.GetRolesAsync(user);
        return new UserSummary(user.Id, user.UserName, user.Email, roles);
    }

    private async Task<AuthResponse> CreateAuthResponseAsync(AppUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var summary = new UserSummary(user.Id, user.UserName, user.Email, roles);
        var token = tokenProvider.GenerateToken(
            new UserTokenData(summary.Id, summary.Email, summary.Roles)
        );

        return new AuthResponse(summary, token);
    }

    private static void EnsureSucceeded(IdentityResult result)
    {
        if (!result.Succeeded)
            throw new OperationException(
                string.Join(", ", result.Errors.Select(error => error.Description))
            );
    }
}
