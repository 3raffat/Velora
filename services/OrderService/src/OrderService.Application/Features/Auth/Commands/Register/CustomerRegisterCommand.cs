using MediatR;
using OrderService.Application.Features.Auth.Dtos;

namespace OrderService.Application.Features.Auth.Commands.Register;

public sealed record CustomerRegisterCommand(string Username, string Email, string Password)
    : IRequest;
