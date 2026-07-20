using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Models;

namespace Velora.Api.Services;

public sealed class CurrentUser(IHttpContextAccessor _httpContextAccessor) : ICurrentUser
{
    public CurrentUserResponse? GetCurrentUser()
    {
        var user = _httpContextAccessor?.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(id, out var userId))
        {
            throw new InvalidOperationException(
                $"Authenticated user is missing a valid '{ClaimTypes.NameIdentifier}' claim."
            );
        }

        var email = user.FindFirstValue(ClaimTypes.Email);
        var name = user.FindFirstValue(ClaimTypes.Name);

        return new CurrentUserResponse(userId, email, name);
    }
}
