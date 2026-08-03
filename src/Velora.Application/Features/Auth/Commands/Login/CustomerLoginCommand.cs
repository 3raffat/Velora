using MediatR;
using Velora.Application.Features.Auth.Dtos;

namespace Velora.Application.Features.Auth.Commands.Login;

public sealed record CustomerLoginCommand(string Email, string Password) : IRequest<LoginUserDto>;
