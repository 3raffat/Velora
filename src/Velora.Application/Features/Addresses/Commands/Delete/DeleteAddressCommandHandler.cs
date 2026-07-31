using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Addresses.Exceptions;
using Velora.Application.Features.Customers.Exceptions;

namespace Velora.Application.Features.Addresses.Commands.Delete;

public sealed class DeleteAddressCommandHandler(
    IVeloraContext _context,
    ILogger<DeleteAddressCommandHandler> _logger
) : IRequestHandler<DeleteAddressCommand>
{
    public async Task Handle(DeleteAddressCommand request, CancellationToken ct)
    {
        var customer = await _context
            .Customers.Include(c => c.Addresses.Where(a => a.Id == request.AddressId))
            .FirstOrDefaultAsync(c => c.Id == request.CustomerId, ct);

        if (customer is null)
            throw new CustomerNotFoundException(request.CustomerId);

        if (!customer.Addresses.Any())
            throw new AddressNotFoundException(request.AddressId);

        customer.RemoveAddress(request.AddressId);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Address {AddressId} deleted for customer {CustomerId}",
            request.AddressId,
            request.CustomerId
        );
    }
}
