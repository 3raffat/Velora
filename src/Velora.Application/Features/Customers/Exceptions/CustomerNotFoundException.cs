using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Customers.Exceptions;

public sealed class CustomerNotFoundException : NotFoundException
{
    public CustomerNotFoundException(Guid id)
        : base($"Customer with Id {id} was not found.") { }
}
