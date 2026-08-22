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
using DeliveryService.Application.Features.Shipments.Queries.GetMyShipments;
using DeliveryService.Application.Features.Shipments.Queries.GetShipments;
using DeliveryService.Application.Features.Shipments.Queries.GetShipmentsByOrderId;
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
    [EndpointSummary("Create a shipment")]
    [EndpointDescription("Creates a new shipment.")]
    [EndpointName("CreateShipment")]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<CreateShipmentDto>),
        StatusCodes.Status201Created
    )]
    public async Task<IActionResult> Create(CreateShipmentRequest request, CancellationToken ct)
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
    [EndpointSummary("Get shipments")]
    [EndpointDescription("Retrieves shipments matching the specified filters.")]
    [EndpointName("GetShipments")]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<IReadOnlyCollection<ShipmentDto>>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetMany(
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

    [HttpGet("order/{orderId:guid}")]
    [AllowAnonymous]
    [EndpointSummary("Get shipments by order")]
    [EndpointDescription("Retrieves all shipments belonging to an order.")]
    [EndpointName("GetShipmentsByOrderId")]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<ShipmentTrackingDto>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetByOrderId(Guid orderId, CancellationToken ct)
    {
        var result = await sender.Send(new GetShipmentsByOrderIdQuery(orderId), ct);

        return Ok(
            new StandardSuccessResponse<ShipmentTrackingDto>(
                result,
                StatusCodes.Status200OK,
                "Shipments retrieved successfully."
            )
        );
    }

    [HttpGet("mine")]
    [Authorize(Roles = nameof(UserRole.Driver))]
    [EndpointSummary("Get my shipments")]
    [EndpointDescription("Retrieves shipments assigned to the authenticated driver.")]
    [EndpointName("GetMyShipments")]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<IReadOnlyCollection<ShipmentDto>>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetMyShipments(CancellationToken ct)
    {
        var driverId =
            currentUser.GetUserId()
            ?? throw new UnauthorizedException("Authenticated driver identity is required.");

        var result = await sender.Send(new GetMyShipmentsQuery(driverId), ct);

        return Ok(
            new StandardSuccessResponse<IReadOnlyCollection<ShipmentDto>>(
                result,
                StatusCodes.Status200OK,
                "Driver shipments retrieved successfully."
            )
        );
    }

    [HttpGet("{shipmentId:guid}/attempts")]
    [EndpointSummary("Get delivery attempts")]
    [EndpointDescription("Retrieves delivery attempts for a shipment.")]
    [EndpointName("GetDeliveryAttempts")]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<IReadOnlyCollection<DeliveryAttemptDto>>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetAttempts(Guid shipmentId, CancellationToken ct)
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
    [EndpointSummary("Assign a shipment driver")]
    [EndpointDescription("Assigns a driver to a shipment.")]
    [EndpointName("AssignShipmentDriver")]
    [ProducesResponseType(typeof(StandardSuccessResponse<ShipmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignDriver(
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
    [Authorize(Roles = nameof(UserRole.Driver))]
    [EndpointSummary("Update shipment status")]
    [EndpointDescription("Updates the status of a shipment.")]
    [EndpointName("UpdateShipmentStatus")]
    [ProducesResponseType(typeof(StandardSuccessResponse<ShipmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeStatus(
        Guid shipmentId,
        UpdateShipmentStatusRequest request,
        CancellationToken ct
    )
    {
        var driverId =
            currentUser.GetUserId()
            ?? throw new UnauthorizedException("Authenticated driver identity is required.");

        var result = await sender.Send(
            new UpdateShipmentStatusCommand(
                shipmentId,
                driverId,
                request.Status,
                request.FailureReason
            ),
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
