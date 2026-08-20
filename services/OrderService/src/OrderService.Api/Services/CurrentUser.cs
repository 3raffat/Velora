using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;

namespace OrderService.Api.Services;

public sealed class CurrentUser(IHttpContextAccessor _httpContextAccessor) : ICurrentUser
{
    public CurrentUserResponse? GetCurrentUser()
    {
        var user = _httpContextAccessor?.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var identityId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(identityId, out var IdentityUserId))
        {
            throw new InvalidOperationException(
                $"Authenticated user is missing a valid '{ClaimTypes.NameIdentifier}' claim."
            );
        }

        var customerId = user.FindFirstValue("customerId")!;

        if (!Guid.TryParse(customerId, out var CustomerId))
        {
            throw new InvalidOperationException(
                $"Authenticated user is missing a valid '{ClaimTypes.NameIdentifier}' claim."
            );
        }

        var email = user.FindFirstValue(ClaimTypes.Email);
        var name = user.FindFirstValue(ClaimTypes.Name);

        return new CurrentUserResponse(CustomerId, IdentityUserId, email, name);
    }
}
