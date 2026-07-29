using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Products.Exceptions;

namespace Velora.Application.Features.Products.Commands.Delete;

public sealed class DeleteProductCommandHandler(
    IVeloraContext _context,
    ILogger<DeleteProductCommandHandler> _logger
) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product is null)
            throw new ProductNotFoundException(request.Id);

        _context.Products.Remove(product);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Product with {productId} deleted successfully", request.Id);
    }
}
