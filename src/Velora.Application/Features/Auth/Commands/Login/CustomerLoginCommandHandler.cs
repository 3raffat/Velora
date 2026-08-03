using MediatR;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Auth.Dtos;

namespace Velora.Application.Features.Auth.Commands.Login;

public sealed class CustomerLoginCommandHandler(
    ILogger<CustomerLoginCommandHandler> _logger,
    IUserService _userService
) : IRequestHandler<CustomerLoginCommand, LoginUserDto>
{
    public async Task<LoginUserDto> Handle(CustomerLoginCommand request, CancellationToken ct)
    {
        var loginUser = await _userService.LoginAsync(request.Email, request.Password, ct);

        _logger.LogInformation(
            "Customer with email {Email} logged in successfully.",
            request.Email
        );
        return loginUser;
    }
}
