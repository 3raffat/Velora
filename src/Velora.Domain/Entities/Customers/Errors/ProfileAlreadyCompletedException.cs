namespace Velora.Domain.Entities.Customers.Errors;

public sealed class ProfileAlreadyCompletedException : Exception
{
    public ProfileAlreadyCompletedException() : base("The customer profile has already been completed")
    {

    }
}
