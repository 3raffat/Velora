using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Models;

namespace Velora.Api.Extensions;

public static class CurrentUserExtension
{
    private static readonly CurrentUserResponse _systemUser = new CurrentUserResponse(
        Guid.Empty,
        "System",
        "System"
    );

    public static CurrentUserResponse GetCurrentUserOrSystem(this ICurrentUser currentUser)
    {
        var user = currentUser.GetCurrentUser() ?? _systemUser;

        return user;
    }
}
