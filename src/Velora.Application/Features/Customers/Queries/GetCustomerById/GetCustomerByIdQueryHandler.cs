using MediatR;
using Microsoft.EntityFrameworkCore;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Customers.Dtos;
using Velora.Application.Features.Customers.Exceptions;
using Velora.Application.Features.Customers.Mappers;

namespace Velora.Application.Features.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken ct)
    {
        var customer = await _context
            .Customers.Include(c => c.Addresses)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (customer is null)
            throw new CustomerNotFoundException(request.Id);

        return customer.ToDto();
    }
}
