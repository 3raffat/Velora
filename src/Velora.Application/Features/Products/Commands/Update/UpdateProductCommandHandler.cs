using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Categories.Exceptions;
using Velora.Application.Features.Products.Exceptions;
using Velora.Domain.Common.ValueObjects;

namespace Velora.Application.Features.Products.Commands.Update;

public sealed class UpdateProductCommandHandler(
    IVeloraContext _context,
    ILogger<UpdateProductCommandHandler> _logger
) : IRequestHandler<UpdateProductCommand>
{
    public async Task Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product is null)
            throw new ProductNotFoundException(request.Id);

        if (product.CategoryId != request.CategoryId)
        {
            var categoryExist = await _context.Categories.AnyAsync(
                c => c.Id == request.CategoryId,
                ct
            );

            if (!categoryExist)
                throw new CategoryNotFoundException(request.CategoryId);
        }

        product.Update(
            Name.Create(request.Name),
            request.Description,
            Money.Create(request.Price),
            request.ImageUrl,
            request.CategoryId
        );

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Product with ID {productId} updated successfully.", product.Id);
    }
}
