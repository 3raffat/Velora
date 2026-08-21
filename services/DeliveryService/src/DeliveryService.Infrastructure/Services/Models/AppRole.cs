using DeliveryService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace DeliveryService.Infrastructure.Services.Models;

public sealed class AppRole : IdentityRole<Guid>
{
    private AppRole() { }

    private AppRole(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        NormalizedName = name.ToUpperInvariant();
    }

    public static AppRole Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidRequestException("Role name is required.");

        return new AppRole(name.Trim());
    }
}
