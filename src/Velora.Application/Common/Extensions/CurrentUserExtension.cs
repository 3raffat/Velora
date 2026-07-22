using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Models;

namespace Velora.Application.Common.Extensions;

public static class CurrentUserExtension
{
    private static readonly Guid SystemUserId = new("11111111-1111-1111-1111-111111111111");
    private static readonly CurrentUserResponse _systemUser = new CurrentUserResponse(
        SystemUserId,
        "System",
        "System"
    );

    public static CurrentUserResponse GetCurrentUserOrSystem(this ICurrentUser currentUser)
    {
        var user = currentUser.GetCurrentUser() ?? _systemUser;

        return user;
    }
}
