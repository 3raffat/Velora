using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Orders.Exceptions;

public sealed class InvalidCancellationChargesException(string message) : DomainException(message);
