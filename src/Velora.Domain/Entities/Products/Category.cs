using Velora.Domain.Common;
using Velora.Domain.Common.ValueObjects;

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
            throw new ArgumentException("Description is required.", nameof(description));

        description = description.Trim();

        if (description.Length < 10)
            throw new ArgumentOutOfRangeException(
                nameof(description),
                "Description must be at least 10 characters."
            );

        if (description.Length > 1000)
            throw new ArgumentOutOfRangeException(
                nameof(description),
                "Description cannot exceed 1000 characters."
            );

        return description;
    }

    public void Update(string description)
    {
        Description = ValidateAndNormalize(description);
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}
