using DeliveryService.Application.Common.Interfaces;
using MediatR;

namespace DeliveryService.Application.Features.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler(IUserService userService)
    : IRequestHandler<GetUsersQuery, IReadOnlyCollection<UserSummary>>
{
    public Task<IReadOnlyCollection<UserSummary>> Handle(
        GetUsersQuery request,
        CancellationToken ct
    ) => userService.GetUsersAsync(request.Role, ct);
}
