using DeliveryService.Application.Common.Enums;
using DeliveryService.Application.Common.Interfaces;
using MediatR;

namespace DeliveryService.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery(UserRole? Role = null)
    : IRequest<IReadOnlyCollection<UserSummary>>;
