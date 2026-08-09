namespace Velora.Application.Common.Models;

public record CurrentUserResponse(
    Guid CustomerId,
    Guid IdentityUserId,
    string? Email,
    string? Name
);
