using MediatR;
using Microsoft.EntityFrameworkCore;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Products.Dtos;
using Velora.Application.Features.Products.Mapper;

namespace Velora.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken ct
    )
    {
        var query = _context.Products.AsNoTracking();

        if (request.CategoryId.HasValue && request.CategoryId.Value != Guid.Empty)
        {
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);
        }
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(p => p.Name.Value.Contains(request.Search));
        }
        var products = await query.ToListAsync(ct);

        return products.ToDtos();
    }
}
