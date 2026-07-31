using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velora.Api.Contracts;
using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Response;
using Velora.Application.Features.Addresses.Commands.Create;
using Velora.Application.Features.Addresses.Commands.Delete;
using Velora.Application.Features.Addresses.Commands.Update;
using Velora.Application.Features.Customers.Commands.CompleteCustomerProfile;

namespace Velora.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:ApiVersion}/customers")]
public class CustomerController(ISender _sender, ICurrentUser _user) : ControllerBase
{
    [HttpPost("me/complete-profile")]
    public async Task<IActionResult> CompleteProfile(
        CompleteCustomerProfileRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(
            new CompleteCustomerProfileCommand(
                request.IdentityId,
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.DateOfBirth
            ),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Customer Profile Completed Successfully"
            )
        );
    }

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

    [HttpPut("{addressId:guid}/addresses")]
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

    [HttpDelete("{addressId:guid}/addresses")]
    public async Task<IActionResult> DeleteCustomerAddress(
        Guid addressId,
        DeleteAddressRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(new DeleteAddressCommand(addressId, request.CustomerId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Address Deleted Successfully"
            )
        );
    }
}
