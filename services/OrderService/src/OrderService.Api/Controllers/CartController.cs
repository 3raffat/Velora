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
using OrderService.Application.Features.ShoppingCarts.Commands.AddCartItem;
using OrderService.Application.Features.ShoppingCarts.Commands.ClearCart;
using OrderService.Application.Features.ShoppingCarts.Commands.RemoveCartItem;
using OrderService.Application.Features.ShoppingCarts.Queries.GetActiveCart;

namespace OrderService.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Tags("Cart-Management")]
[Route("api/v{version:apiVersion}/carts")]
public class CartController(ISender _sender, ICurrentUser _currentUser) : ControllerBase
{
    [HttpGet("my-cart")]
    [EndpointName("GetMyCart")]
    [EndpointSummary("Get active cart")]
    [EndpointDescription("Gets the current customer's active shopping cart.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyCart(CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        var cart = await _sender.Send(new GetActiveCartQuery(user.CustomerId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                cart,
                StatusCodes.Status200OK,
                "Active Cart Retrieved Successfully"
            )
        );
    }

    [HttpPost("items")]
    [EndpointName("AddCartItem")]
    [EndpointSummary("Add cart item")]
    [EndpointDescription("Adds a product to the current customer's shopping cart.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCartItem(AddCartItemRequest request, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(
            new AddCartItemCommand(user.CustomerId, request.ProductId, request.Quantity),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Product Added to cart Successfully"
            )
        );
    }

    [HttpDelete("{cartId:guid}/items")]
    [EndpointName("RemoveCartItem")]
    [EndpointSummary("Remove cart item")]
    [EndpointDescription("Removes a product from a shopping cart.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveCartItem(
        Guid cartId,
        RemoveCartItemRequest request,
        CancellationToken ct
    )
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(
            new RemoveCartItemCommand(user.CustomerId, cartId, request.ProductId),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Product Removed from cart Successfully"
            )
        );
    }

    [HttpPut("{cartId:guid}/clear")]
    [EndpointName("ClearCart")]
    [EndpointSummary("Clear cart")]
    [EndpointDescription("Removes all items from a shopping cart.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ClearCartItem(Guid cartId, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(new ClearCartCommand(user.CustomerId, cartId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Cart Cleared Successfully"
            )
        );
    }
}
