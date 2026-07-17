using Velora.Domain.Common;

namespace Velora.Domain.Entities.Customers.Errors;

public sealed class InvalidIdentityUserIdException : DomainException
{
    public InvalidIdentityUserIdException() : base("Identity user ID cannot be empty.")
    {

    }

}
