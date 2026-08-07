using MediatR;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Auth.Dtos;
using Velora.Domain.Entities.Customers;

namespace Velora.Application.Features.Auth.Commands.Register;

public sealed class CustomerRegisterCommandHandler(
    IVeloraContext _context,
    IUserService _userService,
    ILogger<CustomerRegisterCommandHandler> _logger
) : IRequestHandler<CustomerRegisterCommand>
{
    public async Task Handle(CustomerRegisterCommand request, CancellationToken ct)
    {
        var user = await _userService.RegisterAsync(
            request.Username,
            request.Email,
            request.Password,
            ct
        );

        var customer = Customer.Create(user.UserId);

        await _context.Customers.AddAsync(customer, ct);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Customer with ID {CustomerId} has been registered successfully.",
            customer.Id
        );
    }
}
