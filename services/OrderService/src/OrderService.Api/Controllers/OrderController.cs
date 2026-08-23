using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OrderService.Api.Contracts;
using OrderService.Application.Common.Extensions;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;
using OrderService.Application.Common.Response;
using OrderService.Application.Features.Orders.Commands.Deliver;
using OrderService.Application.Features.Orders.Commands.Ship;
using OrderService.Application.Features.Orders.Queries.GetAllOrders;
using OrderService.Application.Features.Orders.Queries.GetCustomerOrders;
using OrderService.Application.Features.Orders.Queries.GetOrderById;
using OrderService.Application.Features.Orders.Queries.GetShipmentByOrderId;
using OrderService.Application.Features.ShoppingCarts.Commands.Checkout;
using OrderService.Infrastructure.Services.Models;

namespace OrderService.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Tags("Order-Management")]
[Route("api/v{version:apiVersion}/orders")]
public class OrderController(ISender _sender, ICurrentUser _currentUser) : ControllerBase
{
    [HttpPost("checkout")]
    // [Authorize(Roles = nameof(UserRole.User) + " , " + nameof(UserRole.Admin))]
    [EndpointName("Checkout")]
    [EndpointSummary("Checkout cart")]
    [EndpointDescription("Creates an order from the current customer's cart.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Checkout(CheckoutRequest request, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        var orderId = await _sender.Send(
            new CheckoutCartCommand(
                user.CustomerId,
                request.CartId,
                request.ShippingAddressId,
                request.BillingAddressId,
                request.PaymentMethod,
                request.ShippingCost,
                request.PromoCode
            ),
            ct
        );

        return CreatedAtAction(
            nameof(GetOrderById),
            new { orderId },
            new StandardSuccessResponse<object>(
                new { OrderId = orderId },
                StatusCodes.Status201Created,
                "Order created successfully"
            )
        );
    }

    [HttpGet("{orderId:guid}")]
    [EndpointName("GetOrderById")]
    [EndpointSummary("Get order by ID")]
    [EndpointDescription("Gets an order by its unique identifier.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderById(Guid orderId, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        var order = await _sender.Send(new GetOrderByIdQuery(user.CustomerId, orderId), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                order,
                StatusCodes.Status200OK,
                "Order retrieved successfully"
            )
        );
    }

    [HttpGet("{orderId:guid}/shipment")]
    [Authorize(Roles = nameof(UserRole.User) + "," + nameof(UserRole.Admin))]
    [EndpointName("GetShipmentByOrderId")]
    [EndpointSummary("Get shipment by order ID")]
    [EndpointDescription("Gets shipment tracking details for an order.")]
    [ProducesResponseType(
        typeof(StandardSuccessResponse<ShipmentTrackingResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetShipmentByOrderId(Guid orderId, CancellationToken ct)
    {
        var shipment = await _sender.Send(new GetShipmentByOrderIdQuery(orderId), ct);

        return Ok(
            new StandardSuccessResponse<ShipmentTrackingResponse>(
                shipment,
                StatusCodes.Status200OK,
                "Shipment retrieved successfully"
            )
        );
    }

    [HttpGet]
    [Authorize(Roles = nameof(UserRole.User))]
    [EndpointName("GetCustomerOrders")]
    [EndpointSummary("Get customer orders")]
    [EndpointDescription("Gets orders belonging to the current customer.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerOrders(CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        var orders = await _sender.Send(new GetCustomerOrdersQuery(user.CustomerId), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                orders,
                StatusCodes.Status200OK,
                "Orders retrieved successfully"
            )
        );
    }

    [HttpGet("all")]
    // [Authorize(Roles = nameof(UserRole.Admin))]
    [AllowAnonymous]
    [EndpointName("GetAllOrders")]
    [EndpointSummary("Get all orders")]
    [EndpointDescription("Gets all orders. Requires the Admin role.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOrders(CancellationToken ct)
    {
        var orders = await _sender.Send(new GetAllOrdersQuery(), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                orders,
                StatusCodes.Status200OK,
                "Orders retrieved successfully"
            )
        );
    }

    [HttpPut("{orderId:guid}/ship")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [EndpointName("ShipOrder")]
    [EndpointSummary("Ship order")]
    [EndpointDescription("Ships an order. Requires the Admin role.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ShipOrder(Guid orderId, CancellationToken ct)
    {
        await _sender.Send(new ShipOrderCommand(orderId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Order shipped successfully"
            )
        );
    }

    [HttpPut("{orderId:guid}/deliver")]
    [EndpointName("DeliverOrder")]
    [EndpointSummary("Deliver order")]
    [EndpointDescription("Marks an order as delivered. Requires the Admin role.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeliverOrder(Guid orderId, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(new DeliverOrderCommand(orderId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Order delivered successfully"
            )
        );
    }
}
