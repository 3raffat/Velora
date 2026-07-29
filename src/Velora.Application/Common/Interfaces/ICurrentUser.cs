using Velora.Application.Common.Models;

namespace Velora.Application.Common.Interfaces;

public interface ICurrentUser
{
    CurrentUserResponse? GetCurrentUser();
}
