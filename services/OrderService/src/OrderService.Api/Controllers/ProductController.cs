using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OrderService.Api.Contracts;
using OrderService.Application.Common.Response;
using OrderService.Application.Features.Products.Commands.Create;
using OrderService.Application.Features.Products.Commands.Delete;
using OrderService.Application.Features.Products.Commands.Update;
using OrderService.Application.Features.Products.Commands.UpdatePrice;
using OrderService.Application.Features.Products.Commands.UpdateStock;
using OrderService.Application.Features.Products.Queries.GetProductById;
using OrderService.Application.Features.Products.Queries.GetProducts;
using OrderService.Infrastructure.Services.Models;

namespace OrderService.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Tags("Product-Management-Admin")]
[Route("api/v{version:ApiVersion}/products")]
public sealed class ProductController(ISender _sender) : ControllerBase
{
    [HttpGet]
    [EndpointName("GetProducts")]
    [EndpointSummary("Get products")]
    [EndpointDescription("Gets products with optional category and search filters.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts(
        [FromQuery] Guid? categoryId,
        [FromQuery] string? search,
        CancellationToken ct
    )
    {
        var result = await _sender.Send(new GetProductsQuery(categoryId, search), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Products Retrieved Successfully"
            )
        );
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetProductById")]
    [EndpointSummary("Get product by ID")]
    [EndpointDescription("Gets a product by its unique identifier.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetProductByIdQuery(id), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Product Retrieved Successfully"
            )
        );
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [EndpointName("CreateProduct")]
    [EndpointSummary("Create product")]
    [EndpointDescription("Creates a new product. Requires the Admin role.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateProductRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateProductCommand(
                request.Name,
                request.Description,
                request.Price,
                request.StockQuantity,
                request.ImageUrl,
                request.CategoryId
            ),
            ct
        );

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status201Created,
                "Product Created Successfully"
            )
        );
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPut("{id:guid}")]
    [EndpointName("UpdateProduct")]
    [EndpointSummary("Update product")]
    [EndpointDescription("Updates the details of an existing product. Requires the Admin role.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateProductRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(
            new UpdateProductCommand(
                id,
                request.Name,
                request.Description,
                request.ImageUrl,
                request.CategoryId
            ),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Product Updated Successfully"
            )
        );
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPatch("{id:guid}/price")]
    [EndpointName("UpdateProductPrice")]
    [EndpointSummary("Update product price")]
    [EndpointDescription("Updates the price of an existing product. Requires the Admin role.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePrice(
        Guid id,
        UpdateProductPriceRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(new UpdatePriceCommand(id, request.Price), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Product price updated successfully"
            )
        );
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPatch("{id:guid}/stock")]
    [EndpointName("UpdateProductStock")]
    [EndpointSummary("Update product stock")]
    [EndpointDescription("Increases or decreases product stock. Requires the Admin role.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStock(
        Guid id,
        UpdateProductStockRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(new UpdateStockCommand(id, request.Quantity, request.Operation), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Product stock updated successfully"
            )
        );
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpDelete("{id:guid}")]
    [EndpointName("DeleteProduct")]
    [EndpointSummary("Delete product")]
    [EndpointDescription("Deletes an existing product. Requires the Admin role.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeleteProductCommand(id), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Product deleted successfully"
            )
        );
    }
}
