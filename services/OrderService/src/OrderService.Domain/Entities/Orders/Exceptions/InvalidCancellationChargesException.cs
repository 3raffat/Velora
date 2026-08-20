using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Orders.Exceptions;

public sealed class InvalidCancellationChargesException(string message) : DomainException(message);
