using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Common.Extensions;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Response;
using OrderService.Application.Features.Orders.Commands.ApproveRefund;
using OrderService.Application.Features.Orders.Commands.CompleteRefund;
using OrderService.Application.Features.Orders.Commands.RejectRefund;
using OrderService.Api.Contracts;

namespace OrderService.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Tags("Refund-Management")]
[Route("api/v{version:apiVersion}/refunds")]
public sealed class RefundController(ISender _sender, ICurrentUser _currentUser) : ControllerBase
{
    [HttpPut("{orderId:guid}/refund/approve")]
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
