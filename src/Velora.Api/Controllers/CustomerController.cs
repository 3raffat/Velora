using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velora.Api.Contracts;
using Velora.Application.Common.Extensions;
using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Response;
using Velora.Application.Features.Addresses.Commands.Create;
using Velora.Application.Features.Addresses.Commands.Delete;
using Velora.Application.Features.Addresses.Commands.Update;
using Velora.Application.Features.Addresses.Queries.GetAddressById;
using Velora.Application.Features.Addresses.Queries.GetCustomerAddresses;
using Velora.Application.Features.Customers.Commands.CompleteCustomerProfile;
using Velora.Application.Features.Customers.Queries.GetCustomerById;
using Velora.Application.Features.Customers.Queries.GetCustomerProfile;

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

        return CreatedAtAction(
            nameof(GetMyProfile),
            null,
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Customer Profile Completed Successfully"
            )
        );
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(CancellationToken ct)
    {
        var currentUser = _user.GetCurrentUserOrSystem();

        var result = await _sender.Send(new GetCustomerProfileQuery(currentUser.Id), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Customer Profile Retrieved Successfully"
            )
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCustomerById(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetCustomerByIdQuery(id), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Customer Profile Retrieved Successfully"
            )
        );
    }

    [HttpGet("{customerId:guid}/addresses")]
    public async Task<IActionResult> GetCustomerAddresses(Guid customerId, CancellationToken ct)
    {
        var result = await _sender.Send(new GetCustomerAddressesQuery(customerId), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Addresses Retrieved Successfully"
            )
        );
    }

    [HttpGet("{customerId:guid}/addresses/{addressId:guid}")]
    public async Task<IActionResult> GetAddressById(
        Guid customerId,
        Guid addressId,
        CancellationToken ct
    )
    {
        var result = await _sender.Send(new GetAddressByIdQuery(addressId, customerId), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Address Retrieved Successfully"
            )
        );
    }

    [HttpPost("addresses")]
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

        return CreatedAtAction(
            nameof(GetAddressById),
            new { customerId = request.CustomerId, addressId = (Guid?)null },
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
