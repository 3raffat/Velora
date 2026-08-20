using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Categories.Commands.Update;
using OrderService.Application.Features.Categories.Exceptions;
using OrderService.Domain.Common.ValueObjects;

namespace OrderService.Application.Features.Category.Commands.Update;

public sealed class UpdateCategoryCommandHandler(
    IVeloraContext _context,
    ILogger<UpdateCategoryCommandHandler> _logger
) : IRequestHandler<UpdateCategoryCommand>
{
    public async Task Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (category is null)
            throw new CategoryNotFoundException(request.Id);

        if (category.Name.Value != request.Name)
        {
            var name = Name.Create(request.Name);

            var exists = await _context.Categories.AnyAsync(
                c => c.Name == name && c.Id != request.Id,
                ct
            );

            if (exists)
                throw CategoryAlreadyExistsException.ByName(request.Name);
        }

        category.Update(request.Name, request.Description);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Category '{CategoryName}' with ID '{CategoryId}' updated successfully.",
            category.Name.Value,
            category.Id
        );
    }
}
