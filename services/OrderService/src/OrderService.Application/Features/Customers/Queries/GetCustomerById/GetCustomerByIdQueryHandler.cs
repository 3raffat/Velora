using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Customers.Dtos;
using OrderService.Application.Features.Customers.Exceptions;
using OrderService.Application.Features.Customers.Mappers;

namespace OrderService.Application.Features.Customers.Queries.GetCustomerById;

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
