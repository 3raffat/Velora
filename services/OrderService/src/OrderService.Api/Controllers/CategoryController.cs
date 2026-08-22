using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OrderService.Api.Contracts;
using OrderService.Application.Common.Response;
using OrderService.Application.Features.Categories.Commands.Create;
using OrderService.Application.Features.Categories.Commands.Update;
using OrderService.Application.Features.Categories.Queries.GetAllCategories;
using OrderService.Application.Features.Categories.Queries.GetCategoryById;

namespace OrderService.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Tags("Category-Management-Admin")]
[Route("api/v{version:ApiVersion}/categories")]
public sealed class CategoryController(ISender _sender) : ControllerBase
{
    [HttpGet]
    [EndpointName("GetAllCategories")]
    [EndpointSummary("Get all categories")]
    [EndpointDescription("Gets all product categories.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _sender.Send(new GetAllCategoriesQuery(), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Categories Retrieved Successfully"
            )
        );
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetCategoryById")]
    [EndpointSummary("Get category by ID")]
    [EndpointDescription("Gets a category by its unique identifier.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetCategoryByIdQuery(id), ct);

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Category Retrieved Successfully"
            )
        );
    }

    [HttpPost]
    [Authorize]
    [EndpointName("CreateCategory")]
    [EndpointSummary("Create category")]
    [EndpointDescription("Creates a new product category.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateCategoryCommand(request.Name, request.Description),
            ct
        );

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status201Created,
                "Category Created Successfully"
            )
        );
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    [EndpointName("UpdateCategory")]
    [EndpointSummary("Update category")]
    [EndpointDescription("Updates an existing product category.")]
    [ProducesResponseType(typeof(StandardSuccessResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateCategoryRequest request,
        CancellationToken ct
    )
    {
        await _sender.Send(new UpdateCategoryCommand(id, request.Name, request.Description), ct);

        return Ok(
            new StandardSuccessResponse<object?>(
                default,
                StatusCodes.Status200OK,
                "Category Updated Successfully"
            )
        );
    }
}
