using DeliveryService.Application.Common.Interfaces;
using MediatR;

namespace DeliveryService.Application.Features.Users.Commands.SetUserActive;

public sealed record SetUserActiveCommand(Guid UserId, bool IsActive) : IRequest<UserSummary>;
