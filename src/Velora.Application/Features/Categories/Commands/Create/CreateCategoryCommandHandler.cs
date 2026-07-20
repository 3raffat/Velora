using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Categories.Dtos;
using Velora.Application.Features.Categories.Exceptions;
using Velora.Application.Features.Categories.Mapper;
using Velora.Domain.Common.ValueObjects;
using CategoryEntity = Velora.Domain.Entities.Products.Category;

namespace Velora.Application.Features.Categories.Commands.Create;

public sealed class CreateCategoryCommandHandler(
    IVeloraContext _context,
    ILogger<CreateCategoryCommandHandler> _logger
) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var name = Name.Create(request.Name);

        var categoryExists = await _context.Categories.AnyAsync(c => c.Name == name, ct);

        if (categoryExists)
            throw CategoryAlreadyExistsException.ByName(request.Name);

        var category = CategoryEntity.Create(Name.Create(request.Name), request.Description);

        await _context.Categories.AddAsync(category, ct);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Category '{CategoryName}' with ID '{CategoryId}' created successfully.",
            category.Name.Value,
            category.Id
        );

        return category.ToDto();
    }
}
