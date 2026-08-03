using MediatR;
using Microsoft.EntityFrameworkCore;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Products.Dtos;
using Velora.Application.Features.Products.Exceptions;
using Velora.Application.Features.Products.Mapper;

namespace Velora.Application.Features.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await _context
            .Products.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product is null)
            throw new ProductNotFoundException(request.Id);

        return product.ToDto();
    }
}
