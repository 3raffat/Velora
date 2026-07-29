namespace Velora.Application.Common.Models;

public record CurrentUserResponse(Guid Id, string? Email, string? Name);
