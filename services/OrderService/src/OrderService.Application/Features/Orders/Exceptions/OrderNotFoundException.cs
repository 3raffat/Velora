using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Orders.Exceptions;

public sealed class OrderNotFoundException(Guid id)
    : NotFoundException($"Order with Id {id} was not found.");
