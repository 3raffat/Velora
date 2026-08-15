using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Velora.Api.Contracts;
using Velora.Application.Common.Response;
using Velora.Application.Features.Categories.Commands.Create;
using Velora.Application.Features.Categories.Commands.Update;
using Velora.Application.Features.Categories.Queries.GetAllCategories;
using Velora.Application.Features.Categories.Queries.GetCategoryById;

namespace Velora.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Tags("Category-Management-Admin")]
[Route("api/v{version:ApiVersion}/categories")]
public sealed class CategoryController(ISender _sender) : ControllerBase
{
    [HttpGet]
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

    [Authorize]
    [HttpPost]
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
