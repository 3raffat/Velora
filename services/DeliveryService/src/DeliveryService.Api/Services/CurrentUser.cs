using System.Security.Claims;
using DeliveryService.Application.Common.Exceptions;
using DeliveryService.Application.Common.Interfaces;

namespace DeliveryService.Api.Services;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid? GetUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
            return null;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");

        if (!Guid.TryParse(userId, out var identityUserId))
            throw new UnauthorizedException(
                $"Authenticated user is missing a valid '{ClaimTypes.NameIdentifier}' claim."
            );

        return identityUserId;
    }
}
