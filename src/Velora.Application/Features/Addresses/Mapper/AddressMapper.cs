using Velora.Application.Features.Addresses.Dtos;
using Velora.Domain.Entities.Customers;

namespace Velora.Application.Features.Addresses.Mapper;

public static class AddressMapper
{
    public static AddressDto ToDto(this Address address)
    {
        return new AddressDto(
            address.Id,
            address.AddressLine1,
            address.AddressLine2,
            address.City,
            address.State,
            address.Country,
            address.CustomerId
        );
    }

    public static IEnumerable<AddressDto> ToDtos(this IEnumerable<Address> addresses)
    {
        return addresses.Select(a => a.ToDto());
    }
}
