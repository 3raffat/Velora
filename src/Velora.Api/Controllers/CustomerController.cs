using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velora.Api.Contracts;
using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Response;
using Velora.Application.Features.Addresses.Commands.Create;
using Velora.Application.Features.Addresses.Commands.Update;

namespace Velora.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:ApiVersion}/customers")]
public class CustomerController(ISender _sender, ICurrentUser _user) : ControllerBase
{
    [HttpPost("/addresses")]
    public async Task<IActionResult> AddCustomerAddress(
        CreateAddressRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(
            new CreateAddressCommand(
                request.AddressLine1,
                request.AddressLine2,
                request.City,
                request.State,
                request.Country,
                request.CustomerId
            ),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Address Added Successfully"
            )
        );
    }

    [HttpPost("{addressId:guid}/addresses")]
    public async Task<IActionResult> UpdateCustomerAddress(
        Guid addressId,
        UpdateAddressRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(
            new UpdateAddressCommand(
                request.AddressLine1,
                request.AddressLine2,
                request.City,
                request.State,
                request.Country,
                request.CustomerId,
                addressId
            ),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Address Updated Successfully"
            )
        );
    }
}
