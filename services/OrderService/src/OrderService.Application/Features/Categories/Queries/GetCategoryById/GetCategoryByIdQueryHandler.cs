using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Categories.Dtos;
using OrderService.Application.Features.Categories.Exceptions;
using OrderService.Application.Features.Categories.Mapper;

namespace OrderService.Application.Features.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken ct)
    {
        var category = await _context
            .Categories.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (category is null)
            throw new CategoryNotFoundException(request.Id);

        return category.ToDto();
    }
}
