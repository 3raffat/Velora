using MediatR;
using Velora.Application.Features.Auth.Dtos;

namespace Velora.Application.Features.Auth.Commands.Register;

public sealed record CustomerRegisterCommand(string Username, string Email, string Password)
    : IRequest;
