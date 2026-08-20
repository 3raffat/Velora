using OrderService.Application.Common.Models;

namespace OrderService.Application.Common.Interfaces;

public interface ICurrentUser
{
    CurrentUserResponse? GetCurrentUser();
}
