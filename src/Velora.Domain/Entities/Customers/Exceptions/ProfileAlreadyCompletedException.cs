using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Customers.Exceptions;

public sealed class ProfileAlreadyCompletedException : DomainException
{
    public ProfileAlreadyCompletedException()
        : base("The customer profile has already been completed") { }
}
