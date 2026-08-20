using MediatR;

namespace OrderService.Application.Features.Orders.Commands.Confirm;

public sealed record ConfirmOrderCommand(Guid CustomerId, Guid OrderId) : IRequest;
