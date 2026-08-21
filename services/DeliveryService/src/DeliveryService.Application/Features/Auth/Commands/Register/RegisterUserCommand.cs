using DeliveryService.Application.Common.Enums;
using DeliveryService.Application.Common.Interfaces;
using MediatR;

namespace DeliveryService.Application.Features.Auth.Commands.Register;

public sealed record RegisterUserCommand(
    string Username,
    string Email,
    string Password,
    UserRole Role = UserRole.User
) : IRequest<AuthResponse>;
