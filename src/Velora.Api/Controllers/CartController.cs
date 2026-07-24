using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velora.Api.Contracts;
using Velora.Application.Common.Extensions;
using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Response;
using Velora.Application.Features.ShoppingCarts.Commands.AddCartItem;
using Velora.Application.Features.ShoppingCarts.Commands.RemoveCartItem;

namespace Velora.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/carts")]
public class CartController(ISender _sender, ICurrentUser _currentUser) : ControllerBase
{
    [HttpPost("items")]
    public async Task<IActionResult> AddCartItem(AddCartItemRequest request, CancellationToken ct)
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(
            new AddCartItemCommand(request.CustomerId, request.ProductId, request.Quantity),
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

    [HttpDelete("items")]
    public async Task<IActionResult> RemoveCartItem(
        RemoveCartItemRequest request,
        CancellationToken ct
    )
    {
        var user = _currentUser.GetCurrentUserOrSystem();

        await _sender.Send(
            new RemoveCartItemCommand(request.CustomerId, request.CartId, request.ProductId),
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
}
