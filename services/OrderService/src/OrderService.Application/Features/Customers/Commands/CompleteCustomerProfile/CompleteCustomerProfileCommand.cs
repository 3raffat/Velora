using MediatR;

namespace OrderService.Application.Features.Customers.Commands.CompleteCustomerProfile;

public record CompleteCustomerProfileCommand(
    Guid IdentityId,
    string firstName,
    string lastName,
    string email,
    string phoneNumber,
    DateOnly dateOfBirth
) : IRequest;
