using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Velora.Api.Contracts;
using Velora.Application.Common.Extensions;
using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Response;
using Velora.Application.Features.Orders.Commands.ApproveCancellation;
using Velora.Application.Features.Orders.Commands.RejectCancellation;
using Velora.Application.Features.Orders.Commands.RequestCancellation;
using Velora.Application.Features.Orders.Queries.GetCancellationByOrderId;

namespace Velora.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Tags("Cancellation-Management")]
[Route("api/v{version:apiVersion}/cancellations")]
public sealed class CancellationController(ISender _sender, ICurrentUser _currentUser)
    : ControllerBase
{
    [HttpGet("{orderId:guid}/cancellation")]
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
