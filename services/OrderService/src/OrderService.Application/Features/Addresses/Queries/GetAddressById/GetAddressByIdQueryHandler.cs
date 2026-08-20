using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Addresses.Dtos;
using OrderService.Application.Features.Addresses.Exceptions;
using OrderService.Application.Features.Addresses.Mapper;

namespace OrderService.Application.Features.Addresses.Queries.GetAddressById;

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
