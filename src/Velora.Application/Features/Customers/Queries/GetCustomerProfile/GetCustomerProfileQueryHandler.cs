using MediatR;
using Microsoft.EntityFrameworkCore;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Customers.Dtos;
using Velora.Application.Features.Customers.Exceptions;
using Velora.Application.Features.Customers.Mappers;

namespace Velora.Application.Features.Customers.Queries.GetCustomerProfile;

public sealed class GetCustomerProfileQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetCustomerProfileQuery, CustomerDto>
{
    public async Task<CustomerDto> Handle(GetCustomerProfileQuery request, CancellationToken ct)
    {
        var customer = await _context
            .Customers.Include(c => c.Addresses)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdentityUserId == request.IdentityUserId, ct);

        if (customer is null)
            throw new CustomerNotFoundException(request.IdentityUserId);

        return customer.ToDto();
    }
}
