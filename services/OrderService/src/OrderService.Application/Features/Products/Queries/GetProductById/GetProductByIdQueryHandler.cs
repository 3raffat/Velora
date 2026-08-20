using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Products.Dtos;
using OrderService.Application.Features.Products.Exceptions;
using OrderService.Application.Features.Products.Mapper;

namespace OrderService.Application.Features.Products.Queries.GetProductById;

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
