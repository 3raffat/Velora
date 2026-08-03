using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velora.Api.Contracts;
using Velora.Application.Common.Response;
using Velora.Application.Features.Products.Commands.Create;
using Velora.Application.Features.Products.Queries.GetProductById;
using Velora.Application.Features.Products.Queries.GetProducts;

namespace Velora.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:ApiVersion}/products")]
public sealed class ProductController(ISender _sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] Guid? categoryId, CancellationToken ct)
    {
        var result = await _sender.Send(new GetProductsQuery(categoryId), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Products Retrieved Successfully"
            )
        );
    }

    [HttpGet("{id:guid}")]
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
            new { id = result },
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status201Created,
                "Product Created Successfully"
            )
        );
    }
}
