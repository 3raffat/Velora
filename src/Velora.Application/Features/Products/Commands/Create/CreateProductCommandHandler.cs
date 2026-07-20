using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Categories.Exceptions;
using Velora.Application.Features.Products.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Products;

namespace Velora.Application.Features.Products.Commands.Create;

public sealed class CreateProductCommandHandler(
    IVeloraContext _context,
    ILogger<CreateProductCommandHandler> _logger
) : IRequestHandler<CreateProductCommand>
{
    public async Task Handle(CreateProductCommand request, CancellationToken ct)
    {
        var name = Name.Create(request.Name);

        var productExist = await _context.Products.AnyAsync(p => p.Name == name, ct);

        if (productExist)
            throw ProductAlreadyExistsException.ByName(request.Name);

        var categoryExists = await _context.Categories.AnyAsync(
            c => c.Id == request.CategoryId,
            ct
        );

        if (!categoryExists)
            throw new CategoryNotFoundException(request.CategoryId);

        var product = Product.Create(
            Name.Create(request.Name),
            request.Description,
            Money.Create(request.Price),
            request.StockQuantity,
            request.ImageUrl,
            request.CategoryId
        );

        await _context.Products.AddAsync(product, ct);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Product with ID {productId} created successfully.", product.Id);
    }
}
