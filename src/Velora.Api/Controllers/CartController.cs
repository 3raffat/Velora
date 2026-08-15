using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Velora.Api.Contracts;
using Velora.Application.Common.Extensions;
using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Response;
using Velora.Application.Features.ShoppingCarts.Commands.AddCartItem;
using Velora.Application.Features.ShoppingCarts.Commands.ClearCart;
using Velora.Application.Features.ShoppingCarts.Commands.RemoveCartItem;
using Velora.Application.Features.ShoppingCarts.Queries.GetActiveCart;

namespace Velora.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/carts")]
public class CartController(ISender _sender, ICurrentUser _currentUser) : ControllerBase
{
    [HttpGet("my-cart")]
    public async Task<IActionResult> GetMyCart(CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        var cart = await _sender.Send(new GetActiveCartQuery(user.IdentityUserId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                cart,
                StatusCodes.Status200OK,
                "Active Cart Retrieved Successfully"
            )
        );
    }

    [HttpPost("items")]
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
    public async Task<IActionResult> RemoveCartItem(
        Guid cartId,
        RemoveCartItemRequest request,
        CancellationToken ct
    )
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(
            new RemoveCartItemCommand(user.IdentityUserId, cartId, request.ProductId),
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
    public async Task<IActionResult> ClearCartItem(Guid cartId, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(new ClearCartCommand(user.IdentityUserId, cartId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Cart Cleared Successfully"
            )
        );
    }
}
