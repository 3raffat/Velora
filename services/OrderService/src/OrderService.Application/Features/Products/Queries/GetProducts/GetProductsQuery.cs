using MediatR;
using OrderService.Application.Features.Products.Dtos;

namespace OrderService.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery(Guid? CategoryId = null, string? Search = null)
    : IRequest<IEnumerable<ProductDto>>;
