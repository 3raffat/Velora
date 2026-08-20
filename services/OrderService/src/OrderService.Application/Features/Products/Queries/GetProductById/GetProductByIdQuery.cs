using MediatR;
using OrderService.Application.Features.Products.Dtos;

namespace OrderService.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;
