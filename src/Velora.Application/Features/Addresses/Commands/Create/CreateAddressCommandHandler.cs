using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Customers.Exceptions;

namespace Velora.Application.Features.Addresses.Commands.Create;

public sealed class CreateAddressCommandHandler(
    IVeloraContext _context,
    ILogger<CreateAddressCommandHandler> _logger
) : IRequestHandler<CreateAddressCommand>
{
    public async Task Handle(CreateAddressCommand request, CancellationToken ct)
    {
        var customer = await _context
            .Customers.Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == request.CustomerId, ct);

        if (customer is null)
            throw new CustomerNotFoundException(request.CustomerId);

        customer.AddAddress(
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.State,
            request.Country
        );

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Address added for customer {CustomerId}", customer.Id);
    }
}
