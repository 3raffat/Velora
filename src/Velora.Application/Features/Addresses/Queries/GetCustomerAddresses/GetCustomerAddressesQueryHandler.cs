using MediatR;
using Microsoft.EntityFrameworkCore;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Addresses.Dtos;
using Velora.Application.Features.Addresses.Mapper;

namespace Velora.Application.Features.Addresses.Queries.GetCustomerAddresses;

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
