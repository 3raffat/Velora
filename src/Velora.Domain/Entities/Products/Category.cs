using Velora.Domain.Common;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Products.Exceptions;

namespace Velora.Domain.Entities.Products;

public sealed class Category : BaseEntity
{
    public Name Name { get; private set; } = null!;
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Category() { }

    private Category(Guid id, Name name, string description)
        : base(id)
    {
        Name = name;
        Description = description;
        IsActive = true;
    }

    public static Category Create(Name name, string description)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        var trimmedDescription = ValidateAndNormalize(description);

        return new Category(Guid.NewGuid(), name, trimmedDescription);
    }

    private static string ValidateAndNormalize(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidDescriptionException();

        description = description.Trim();

        if (description.Length < 10)
            throw new InvalidDescriptionException(10, true);

        if (description.Length > 500)
            throw new InvalidDescriptionException(500);

        return description;
    }

    public void Update(string name, string description)
    {
        Name = Name.Create(name);

        Description = ValidateAndNormalize(description);
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}
