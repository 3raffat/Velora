using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Addresses.Exceptions;
using OrderService.Application.Features.Customers.Exceptions;

namespace OrderService.Application.Features.Addresses.Commands.Update;

public sealed class UpdateAddressCommandHandler(
    IVeloraContext _context,
    ILogger<UpdateAddressCommandHandler> _logger
) : IRequestHandler<UpdateAddressCommand>
{
    public async Task Handle(UpdateAddressCommand request, CancellationToken ct)
    {
        var customer = await _context
            .Customers.Include(c => c.Addresses.Where(a => a.Id == request.AddressId))
            .FirstOrDefaultAsync(c => c.Id == request.CustomerId, ct);

        if (customer is null)
            throw new CustomerNotFoundException(request.CustomerId);

        if (!customer.Addresses.Any())
            throw new AddressNotFoundException(request.AddressId);

        customer.UpdateAddress(
            request.AddressId,
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.State,
            request.Country
        );

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Address {AddressId} updated successfully for customer {CustomerId}",
            request.AddressId,
            request.CustomerId
        );
    }
}
