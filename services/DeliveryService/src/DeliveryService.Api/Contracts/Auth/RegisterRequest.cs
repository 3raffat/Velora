using DeliveryService.Application.Common.Enums;

namespace DeliveryService.Api.Contracts.Auth;

public sealed record RegisterRequest(
    string Username,
    string Email,
    string Password,
    UserRole Role = UserRole.User
);
