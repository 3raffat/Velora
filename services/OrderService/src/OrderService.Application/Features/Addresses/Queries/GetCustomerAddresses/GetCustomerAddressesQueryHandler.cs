using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Addresses.Dtos;
using OrderService.Application.Features.Addresses.Mapper;

namespace OrderService.Application.Features.Addresses.Queries.GetCustomerAddresses;

public sealed class GetCustomerAddressesQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetCustomerAddressesQuery, IEnumerable<AddressDto>>
{
    public async Task<IEnumerable<AddressDto>> Handle(
        GetCustomerAddressesQuery request,
        CancellationToken ct
    )
    {
        var addresses = await _context
            .Addresses.AsNoTracking()
            .Where(a => a.CustomerId == request.CustomerId)
            .ToListAsync(ct);

        return addresses.ToDtos();
    }
}
