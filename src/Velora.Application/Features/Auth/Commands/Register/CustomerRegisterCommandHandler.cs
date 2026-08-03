using MediatR;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Auth.Dtos;

namespace Velora.Application.Features.Auth.Commands.Register;

public sealed class CustomerRegisterCommandHandler(IUserService _userService)
    : IRequestHandler<CustomerRegisterCommand>
{
    public async Task Handle(CustomerRegisterCommand request, CancellationToken cancellationToken)
    {
        await _userService.RegisterAsync(
            request.Username,
            request.Email,
            request.Password,
            cancellationToken
        );
    }
}
