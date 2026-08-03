using MediatR;
using Microsoft.EntityFrameworkCore;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Addresses.Dtos;
using Velora.Application.Features.Addresses.Exceptions;
using Velora.Application.Features.Addresses.Mapper;

namespace Velora.Application.Features.Addresses.Queries.GetAddressById;

public sealed class GetAddressByIdQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetAddressByIdQuery, AddressDto>
{
    public async Task<AddressDto> Handle(GetAddressByIdQuery request, CancellationToken ct)
    {
        var address = await _context
            .Addresses.AsNoTracking()
            .FirstOrDefaultAsync(
                a => a.Id == request.AddressId && a.CustomerId == request.CustomerId,
                ct
            );

        if (address is null)
            throw new AddressNotFoundException(request.AddressId);

        return address.ToDto();
    }
}
