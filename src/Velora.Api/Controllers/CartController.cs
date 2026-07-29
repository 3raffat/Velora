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
using Velora.Domain.Entities.ShoppingCart;

namespace Velora.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/carts")]
public class CartController(ISender _sender, ICurrentUser _currentUser) : ControllerBase
{
    [HttpPost("{cartId:guid}/items")]
    public async Task<IActionResult> AddCartItem(
        Guid cartId,
        AddCartItemRequest request,
        CancellationToken ct
    )
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(
            new AddCartItemCommand(user.Id, cartId, request.ProductId, request.Quantity),
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

        await _sender.Send(new RemoveCartItemCommand(user.Id, cartId, request.ProductId), ct);

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

        await _sender.Send(new ClearCartCommand(user.Id, cartId), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Cart Cleared Successfully"
            )
        );
    }
}
