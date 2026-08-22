using Asp.Versioning;
using DeliveryService.Api.Contracts.Shipments;
using DeliveryService.Application.Common.Enums;
using DeliveryService.Application.Common.Exceptions;
using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Application.Common.Response;
using DeliveryService.Application.Features.DeliveryAttempts.Queries.GetDeliveryAttempts;
using DeliveryService.Application.Features.Shipments.Commands.AssignShipmentDriver;
using DeliveryService.Application.Features.Shipments.Commands.CreateShipment;
using DeliveryService.Application.Features.Shipments.Commands.UpdateShipmentStatus;
using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Application.Features.Shipments.Queries.GetShipments;
using DeliveryService.Domain.Common.ValueObjects;
using DeliveryService.Domain.Common.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Authorize]
[Route("api/v{version:ApiVersion}/shipments")]
public sealed class ShipmentsController(ISender sender, ICurrentUser currentUser) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<CreateShipmentDto>),
        StatusCodes.Status201Created
    )]
    public async Task<ActionResult<StandardSuccessResponse<CreateShipmentDto>>> Create(
        CreateShipmentRequest request,
        CancellationToken ct
    )
    {
        var result = await sender.Send(
            new CreateShipmentCommand(
                request.OrderId,
                request.RecipientName,
                request.RecipientPhone,
                AddressSnapshot.Create(
                    request.AddressLine1,
                    request.AddressLine2,
                    request.City,
                    request.State,
                    request.Country
                ),
                request.TotalAmount
            ),
            ct
        );

        return StatusCode(
            StatusCodes.Status201Created,
            new StandardSuccessResponse<CreateShipmentDto>(
                result,
                StatusCodes.Status201Created,
                "Shipment created successfully."
            )
        );
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<IReadOnlyCollection<ShipmentDto>>),
        StatusCodes.Status200OK
    )]
    public async Task<
        ActionResult<StandardSuccessResponse<IReadOnlyCollection<ShipmentDto>>>
    > GetMany(
        [FromQuery] Guid? orderId,
        [FromQuery] string? trackingNumber,
        [FromQuery] Guid? driverId,
        CancellationToken ct
    )
    {
        var result = await sender.Send(
            new GetShipmentsQuery(orderId, trackingNumber, driverId),
            ct
        );

        return Ok(
            new StandardSuccessResponse<IReadOnlyCollection<ShipmentDto>>(
                result,
                StatusCodes.Status200OK,
                "Shipments retrieved successfully."
            )
        );
    }

    [HttpGet("mine")]
    [Authorize(Roles = nameof(UserRole.Driver))]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<IReadOnlyCollection<ShipmentDto>>),
        StatusCodes.Status200OK
    )]
    public async Task<
        ActionResult<StandardSuccessResponse<IReadOnlyCollection<ShipmentDto>>>
    > GetMine(CancellationToken ct)
    {
        var driverId =
            currentUser.GetUserId()
            ?? throw new UnauthorizedException("Authenticated driver identity is required.");

        var result = await sender.Send(new GetShipmentsQuery(DriverId: driverId), ct);

        return Ok(
            new StandardSuccessResponse<IReadOnlyCollection<ShipmentDto>>(
                result,
                StatusCodes.Status200OK,
                "Driver shipments retrieved successfully."
            )
        );
    }

    [HttpGet("{shipmentId:guid}/attempts")]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<IReadOnlyCollection<DeliveryAttemptDto>>),
        StatusCodes.Status200OK
    )]
    public async Task<
        ActionResult<StandardSuccessResponse<IReadOnlyCollection<DeliveryAttemptDto>>>
    > GetAttempts(Guid shipmentId, CancellationToken ct)
    {
        var result = await sender.Send(new GetDeliveryAttemptsQuery(shipmentId), ct);

        return Ok(
            new StandardSuccessResponse<IReadOnlyCollection<DeliveryAttemptDto>>(
                result,
                StatusCodes.Status200OK,
                "Delivery attempts retrieved successfully."
            )
        );
    }

    [HttpPatch("{shipmentId:guid}/driver")]
    [Authorize(Roles = nameof(UserRole.Dispatcher) + "," + nameof(UserRole.DeliveryAdmin))]
    [ProducesResponseType(typeof(StandardSuccessResponse<ShipmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<StandardSuccessResponse<ShipmentDto>>> AssignDriver(
        Guid shipmentId,
        AssignShipmentDriverRequest request,
        CancellationToken ct
    )
    {
        var result = await sender.Send(
            new AssignShipmentDriverCommand(shipmentId, request.DriverId),
            ct
        );

        return Ok(
            new StandardSuccessResponse<ShipmentDto>(
                result,
                StatusCodes.Status200OK,
                "Driver assigned successfully."
            )
        );
    }

    [HttpPatch("{shipmentId:guid}/status")]
    [ProducesResponseType(typeof(StandardSuccessResponse<ShipmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<StandardSuccessResponse<ShipmentDto>>> ChangeStatus(
        Guid shipmentId,
        UpdateShipmentStatusRequest request,
        CancellationToken ct
    )
    {
        var result = await sender.Send(
            new UpdateShipmentStatusCommand(shipmentId, request.Status, request.FailureReason),
            ct
        );

        return Ok(
            new StandardSuccessResponse<ShipmentDto>(
                result,
                StatusCodes.Status200OK,
                "Shipment status updated successfully."
            )
        );
    }
}
