using MediatR;
using Velora.Application.Features.Products.Dtos;

namespace Velora.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery(Guid? CategoryId = null)
    : IRequest<IReadOnlyCollection<ProductDto>>;
