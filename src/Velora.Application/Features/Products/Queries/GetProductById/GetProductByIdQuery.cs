using MediatR;
using Velora.Application.Features.Products.Dtos;

namespace Velora.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;
