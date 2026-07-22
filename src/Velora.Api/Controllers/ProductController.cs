using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velora.Api.Contracts;
using Velora.Application.Common.Response;
using Velora.Application.Features.Products.Commands.Create;

namespace Velora.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:ApiVersion}/products")]
public sealed class ProductController(ISender _sender) : ControllerBase
{
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

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Product Created Successfully"
            )
        );
    }
}
