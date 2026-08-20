using MediatR;
using OrderService.Application.Features.Auth.Dtos;

namespace OrderService.Application.Features.Auth.Commands.Login;

public sealed record CustomerLoginCommand(string Email, string Password) : IRequest<LoginUserDto>;
