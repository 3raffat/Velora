using MediatR;

namespace Velora.Application.Features.Auth.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(string UserId, string Token) : IRequest;
