using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Customers.Exceptions;

public sealed class MaxAddressesReachedException(int maxAddresses)
    : DomainException($"A customer cannot have more than {maxAddresses} addresses.");
