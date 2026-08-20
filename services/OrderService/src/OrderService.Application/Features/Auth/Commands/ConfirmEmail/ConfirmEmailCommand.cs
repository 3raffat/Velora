using MediatR;

namespace OrderService.Application.Features.Auth.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(string UserId, string Token) : IRequest;
