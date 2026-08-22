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
using OrderService.Application.Features.Orders.Commands.ApproveRefund;
using OrderService.Application.Features.Orders.Commands.CompleteRefund;
using OrderService.Application.Features.Orders.Commands.RejectRefund;

namespace OrderService.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Tags("Refund-Management")]
[Route("api/v{version:apiVersion}/refunds")]
public sealed class RefundController(ISender _sender, ICurrentUser _currentUser) : ControllerBase
{
    [HttpPut("{orderId:guid}/refund/approve")]
    [EndpointName("ApproveRefund")]
    [EndpointSummary("Approve refund")]
    [EndpointDescription("Approves a refund for an order.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ApproveRefund(Guid orderId, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(new ApproveRefundCommand(user.IdentityUserId, orderId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Refund approved successfully"
            )
        );
    }

    [HttpPut("{orderId:guid}/refund/complete")]
    [EndpointName("CompleteRefund")]
    [EndpointSummary("Complete refund")]
    [EndpointDescription("Completes a refund using the transaction identifier.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CompleteRefund(
        Guid orderId,
        CompleteRefundRequest request,
        CancellationToken ct
    )
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(
            new CompleteRefundCommand(user.IdentityUserId, orderId, request.TransactionId),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Refund completed successfully"
            )
        );
    }

    [HttpPut("{orderId:guid}/refund/reject")]
    [EndpointName("RejectRefund")]
    [EndpointSummary("Reject refund")]
    [EndpointDescription("Rejects a refund for an order.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RejectRefund(
        Guid orderId,
        RejectRefundRequest request,
        CancellationToken ct
    )
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(
            new RejectRefundCommand(user.IdentityUserId, orderId, request.Reason),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Refund rejected successfully"
            )
        );
    }
}
