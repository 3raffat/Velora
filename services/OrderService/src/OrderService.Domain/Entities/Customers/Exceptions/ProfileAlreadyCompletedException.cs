using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Customers.Exceptions;

public sealed class ProfileAlreadyCompletedException : DomainException
{
    public ProfileAlreadyCompletedException()
        : base("The customer profile has already been completed") { }
}
