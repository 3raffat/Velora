using MediatR;

namespace Velora.Application.Features.Orders.Commands.Confirm;

public sealed record ConfirmOrderCommand(Guid CustomerId, Guid OrderId) : IRequest;
