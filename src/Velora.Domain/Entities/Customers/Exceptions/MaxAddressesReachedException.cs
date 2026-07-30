using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Customers.Exceptions;

public sealed class MaxAddressesReachedException(int maxAddresses)
    : DomainException($"A customer cannot have more than {maxAddresses} addresses.");
