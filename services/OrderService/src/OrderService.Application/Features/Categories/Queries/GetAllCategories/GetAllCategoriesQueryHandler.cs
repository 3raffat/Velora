using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Categories.Dtos;
using OrderService.Application.Features.Categories.Mapper;

namespace OrderService.Application.Features.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken ct
    )
    {
        var categories = await _context.Categories.AsNoTracking().ToListAsync(ct);

        return categories.ToDtos();
    }
}
