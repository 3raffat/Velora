using DeliveryService.Application.Common.Interfaces;
using MediatR;

namespace DeliveryService.Application.Features.Auth.Commands.Login;

public sealed record LoginUserCommand(string Email, string Password) : IRequest<AuthResponse>;
