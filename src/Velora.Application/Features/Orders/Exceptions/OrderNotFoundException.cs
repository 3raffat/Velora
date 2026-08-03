using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Orders.Exceptions;

public sealed class OrderNotFoundException(Guid id)
    : NotFoundException($"Order with Id {id} was not found.");
