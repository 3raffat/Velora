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
using OrderService.Application.Features.Orders.Commands.ApproveCancellation;
using OrderService.Application.Features.Orders.Commands.RejectCancellation;
using OrderService.Application.Features.Orders.Commands.RequestCancellation;
using OrderService.Application.Features.Orders.Queries.GetCancellationByOrderId;

namespace OrderService.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Tags("Cancellation-Management")]
[Route("api/v{version:apiVersion}/cancellations")]
public sealed class CancellationController(ISender _sender, ICurrentUser _currentUser)
    : ControllerBase
{
    [HttpGet("{orderId:guid}/cancellation")]
    [EndpointName("GetCancellation")]
    [EndpointSummary("Get cancellation")]
    [EndpointDescription("Gets the cancellation for an order.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCancellation(Guid orderId, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        var cancellation = await _sender.Send(
            new GetCancellationByOrderIdQuery(user.IdentityUserId, orderId),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object>(
                cancellation,
                StatusCodes.Status200OK,
                "Cancellation retrieved successfully"
            )
        );
    }

    [HttpPut("{orderId:guid}/cancellation/approve")]
    [EndpointName("ApproveCancellation")]
    [EndpointSummary("Approve cancellation")]
    [EndpointDescription("Approves an order cancellation request.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ApproveCancellation(
        Guid orderId,
        ApproveCancellationRequest request,
        CancellationToken ct
    )
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(
            new ApproveCancellationCommand(user.CustomerId, orderId, request.CancellationCharges),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Cancellation approved successfully"
            )
        );
    }

    [HttpPut("{orderId:guid}/cancellation/reject")]
    [EndpointName("RejectCancellation")]
    [EndpointSummary("Reject cancellation")]
    [EndpointDescription("Rejects an order cancellation request.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RejectCancellation(
        Guid orderId,
        RejectCancellationRequest request,
        CancellationToken ct
    )
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(
            new RejectCancellationCommand(user.IdentityUserId, orderId, request.Remarks),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Cancellation rejected successfully"
            )
        );
    }

    [HttpPost("{orderId:guid}/cancellation/request")]
    [EndpointName("RequestCancellation")]
    [EndpointSummary("Request cancellation")]
    [EndpointDescription("Submits a cancellation request for an order.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RequestCancellation(
        Guid orderId,
        RequestCancellationRequest request,
        CancellationToken ct
    )
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(
            new RequestCancellationCommand(orderId, user.CustomerId, request.Reason),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Cancellation request submitted successfully"
            )
        );
    }
}
