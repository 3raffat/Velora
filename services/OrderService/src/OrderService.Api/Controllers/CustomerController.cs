using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OrderService.Api.Contracts;
using OrderService.Application.Common.Extensions;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Response;
using OrderService.Application.Features.Addresses.Commands.Create;
using OrderService.Application.Features.Addresses.Commands.Delete;
using OrderService.Application.Features.Addresses.Commands.Update;
using OrderService.Application.Features.Addresses.Queries.GetAddressById;
using OrderService.Application.Features.Addresses.Queries.GetCustomerAddresses;
using OrderService.Application.Features.Customers.Commands.CompleteCustomerProfile;
using OrderService.Application.Features.Customers.Queries.GetCustomerById;
using OrderService.Application.Features.Customers.Queries.GetCustomerProfile;

namespace OrderService.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Tags("Customer-Management")]
[Route("api/v{version:ApiVersion}/customers")]
public class CustomerController(ISender _sender, ICurrentUser _user) : ControllerBase
{
    [HttpPost("me/complete-profile")]
    [EndpointName("CompleteCustomerProfile")]
    [EndpointSummary("Complete customer profile")]
    [EndpointDescription("Completes the current customer's profile.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status201Created)]
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
    [EndpointName("GetMyCustomerProfile")]
    [EndpointSummary("Get my profile")]
    [EndpointDescription("Gets the current customer's profile.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyProfile(CancellationToken ct)
    {
        var currentUser = _user.GetCurrentUserOrSystem();

        var result = await _sender.Send(
            new GetCustomerProfileQuery(currentUser.IdentityUserId),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Customer Profile Retrieved Successfully"
            )
        );
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetCustomerById")]
    [EndpointSummary("Get customer by ID")]
    [EndpointDescription("Gets a customer by their unique identifier.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
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

    [HttpGet("addresses")]
    [EndpointName("GetCustomerAddresses")]
    [EndpointSummary("Get customer addresses")]
    [EndpointDescription("Gets the current customer's saved addresses.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerAddresses(CancellationToken ct)
    {
        var currentUser = _user.GetCurrentUserOrSystem();

        var result = await _sender.Send(new GetCustomerAddressesQuery(currentUser.CustomerId), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Addresses Retrieved Successfully"
            )
        );
    }

    [HttpGet("addresses/{addressId:guid}")]
    [EndpointName("GetAddressById")]
    [EndpointSummary("Get address by ID")]
    [EndpointDescription("Gets a saved address by its unique identifier.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAddressById(Guid addressId, CancellationToken ct)
    {
        var user = _user.GetCurrentUserOrSystem();

        var result = await _sender.Send(new GetAddressByIdQuery(addressId, user.CustomerId), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Address Retrieved Successfully"
            )
        );
    }

    [Authorize]
    [HttpPost("addresses")]
    [EndpointName("AddCustomerAddress")]
    [EndpointSummary("Add customer address")]
    [EndpointDescription("Adds a saved address for the current customer.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddCustomerAddress(
        CreateAddressRequest request,
        CancellationToken ct
    )
    {
        var user = _user.GetCurrentUserOrSystem();

        var result = await _sender.Send(
            new CreateAddressCommand(
                request.AddressLine1,
                request.AddressLine2,
                request.City,
                request.State,
                request.Country,
                user.CustomerId
            ),
            ct
        );

        return CreatedAtAction(
            nameof(GetAddressById),
            new { addressId = result.Id },
            new StandardSuccessResponse<object?>(
                result,
                StatusCodes.Status200OK,
                "Address Added Successfully"
            )
        );
    }

    [Authorize]
    [HttpPut("{addressId:guid}/addresses")]
    [EndpointName("UpdateCustomerAddress")]
    [EndpointSummary("Update customer address")]
    [EndpointDescription("Updates a saved address for the current customer.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCustomerAddress(
        Guid addressId,
        UpdateAddressRequest request,
        CancellationToken ct
    )
    {
        var user = _user.GetCurrentUserOrSystem();

        await _sender.Send(
            new UpdateAddressCommand(
                request.AddressLine1,
                request.AddressLine2,
                request.City,
                request.State,
                request.Country,
                user.CustomerId,
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

    [Authorize]
    [HttpDelete("{addressId:guid}/addresses")]
    [EndpointName("DeleteCustomerAddress")]
    [EndpointSummary("Delete customer address")]
    [EndpointDescription("Deletes a saved address for the current customer.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCustomerAddress(Guid addressId, CancellationToken ct)
    {
        var user = _user.GetCurrentUserOrSystem();

        await _sender.Send(new DeleteAddressCommand(addressId, user.CustomerId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Address Deleted Successfully"
            )
        );
    }
}
