using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Velora.Api.Contracts;
using Velora.Application.Common.Extensions;
using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Response;
using Velora.Application.Features.Orders.Commands.Confirm;
using Velora.Application.Features.Orders.Commands.Deliver;
using Velora.Application.Features.Orders.Commands.Ship;
using Velora.Application.Features.Orders.Queries.GetCustomerOrders;
using Velora.Application.Features.Orders.Queries.GetOrderById;
using Velora.Application.Features.ShoppingCarts.Commands.Checkout;

namespace Velora.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Tags("Order-Management")]
[Route("api/v{version:apiVersion}/orders")]
public class OrderController(ISender _sender, ICurrentUser _currentUser) : ControllerBase
{
    [HttpPost("checkout")]
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
    public async Task<IActionResult> GetOrderById(Guid orderId, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        var order = await _sender.Send(new GetOrderByIdQuery(user.IdentityUserId, orderId), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                order,
                StatusCodes.Status200OK,
                "Order retrieved successfully"
            )
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomerOrders(CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        var orders = await _sender.Send(new GetCustomerOrdersQuery(user.IdentityUserId), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                orders,
                StatusCodes.Status200OK,
                "Orders retrieved successfully"
            )
        );
    }

    [HttpPut("{orderId:guid}/confirm")]
    public async Task<IActionResult> ConfirmOrder(Guid orderId, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(new ConfirmOrderCommand(user.IdentityUserId, orderId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Order confirmed successfully"
            )
        );
    }

    [HttpPut("{orderId:guid}/ship")]
    public async Task<IActionResult> ShipOrder(Guid orderId, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(new ShipOrderCommand(user.IdentityUserId, orderId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Order shipped successfully"
            )
        );
    }

    [HttpPut("{orderId:guid}/deliver")]
    public async Task<IActionResult> DeliverOrder(Guid orderId, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(new DeliverOrderCommand(user.IdentityUserId, orderId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Order delivered successfully"
            )
        );
    }
}
