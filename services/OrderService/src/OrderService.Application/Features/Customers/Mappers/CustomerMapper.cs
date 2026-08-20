using OrderService.Application.Features.Addresses.Mapper;
using OrderService.Application.Features.Customers.Dtos;
using OrderService.Domain.Entities.Customers;

namespace OrderService.Application.Features.Customers.Mappers;

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
