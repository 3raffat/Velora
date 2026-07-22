using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Customers.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Customers.ValueObjects;

namespace Velora.Application.Features.Customers.Commands.CompleteCustomerProfile;

public sealed class CompleteCustomerProfileCommandHandler(
    IVeloraContext _context,
    ILogger<CompleteCustomerProfileCommandHandler> _logger
) : IRequestHandler<CompleteCustomerProfileCommand>
{
    public async Task Handle(CompleteCustomerProfileCommand request, CancellationToken ct)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(
            c => c.IdentityUserId == request.IdentityId,
            ct
        );

        if (customer is null)
            throw new CustomerNotFoundException(request.IdentityId);

        customer.CompleteProfile(
            Name.Create(request.firstName),
            Name.Create(request.lastName),
            Email.Create(request.email),
            PhoneNumber.Create(request.phoneNumber),
            request.dateOfBirth
        );

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Customer with Id {CustomerId} completed profile successfully",
            request.IdentityId
        );
    }
}
