using DeliveryService.Application.Common.Interfaces;
using MediatR;

namespace DeliveryService.Application.Features.Users.Commands.SetUserActive;

public sealed class SetUserActiveCommandHandler(IUserService userService)
    : IRequestHandler<SetUserActiveCommand, UserSummary>
{
    public Task<UserSummary> Handle(SetUserActiveCommand request, CancellationToken ct) =>
        userService.SetActiveAsync(request.UserId, request.IsActive, ct);
}
