using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Velora.Api.Contracts;
using Velora.Application.Common.Response;
using Velora.Application.Features.Categories.Commands.Create;
using Velora.Application.Features.Categories.Commands.Update;

namespace Velora.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:ApiVersion}/categories")]
public sealed class CategoryController(ISender _sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateCategoryCommand(request.Name, request.Description),
            ct
        );

        return Ok(
            new StandardSuccessResponse<object>(
                result,
                StatusCodes.Status200OK,
                "Category Created Successfully"
            )
        );
    }

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
