using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Customers.Exceptions;

public sealed class CustomerNotFoundException : NotFoundException
{
    public CustomerNotFoundException(Guid id)
        : base($"Customer with Id {id} was not found.") { }
}
