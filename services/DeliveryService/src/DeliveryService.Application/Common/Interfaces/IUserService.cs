using DeliveryService.Application.Common.Enums;

namespace DeliveryService.Application.Common.Interfaces;

public interface IUserService
{
    Task<AuthResponse> RegisterAsync(
        string username,
        string email,
        string password,
        UserRole role,
        CancellationToken ct = default
    );
    Task<AuthResponse> LoginAsync(string email, string password, CancellationToken ct = default);
    Task<bool> IsDriverAsync(Guid userId, CancellationToken ct = default);
    Task<UserSummary?> GetUserByIdAsync(Guid userId, CancellationToken ct = default);
    Task<IReadOnlyCollection<UserSummary>> GetUsersAsync(
        UserRole? role = null,
        CancellationToken ct = default
    );
    Task<UserSummary> SetActiveAsync(Guid userId, bool isActive, CancellationToken ct = default);
}

public sealed record UserSummary(
    Guid Id,
    string? UserName,
    string? Email,
    IList<string> Roles,
    bool IsActive
);

public sealed record AuthResponse(UserSummary User, TokenResult Token);
