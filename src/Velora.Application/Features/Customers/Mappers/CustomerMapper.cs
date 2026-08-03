using Velora.Application.Features.Addresses.Mapper;
using Velora.Application.Features.Customers.Dtos;
using Velora.Domain.Entities.Customers;

namespace Velora.Application.Features.Customers.Mappers;

public static class CustomerMapper
{
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.IdentityUserId,
            customer.FirstName?.Value,
            customer.LastName?.Value,
            customer.Email?.Value,
            customer.PhoneNumber?.Value,
            customer.IsProfileCompleted ? customer.DateOfBirth : null,
            customer.IsProfileCompleted,
            customer.Addresses?.ToDtos()
        );
    }
}
