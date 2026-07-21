using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Orders;
using Velora.Domain.Entities.Products.Exceptions;

namespace Velora.Domain.Entities.Products;

public class Product : AuditableEntity, ISoftDelete
{
    public Name Name { get; private set; } = null!;
    public string Description { get; private set; } = string.Empty;
    public Money Price { get; private set; } = null!;
    public int StockQuantity { get; private set; }
    public string? ImageUrl { get; private set; }
    public bool IsAvailable { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private readonly List<Feedback> _feedbacks = new();
    public IReadOnlyCollection<Feedback> Feedbacks => _feedbacks.AsReadOnly();

    private Product() { }

    private Product(
        Guid id,
        Name name,
        string description,
        Money price,
        int stockQuantity,
        string? imageUrl,
        Guid categoryId
    )
        : base(id)
    {
        Name = name;
        Description = description.Trim();
        Price = price;
        StockQuantity = stockQuantity;
        ImageUrl = imageUrl?.Trim();
        CategoryId = categoryId;
        IsAvailable = stockQuantity > 0;
    }

    public static Product Create(
        Name name,
        string description,
        Money price,
        int stockQuantity,
        string? imageUrl,
        Guid categoryId
    )
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidDescriptionException();

        if (description.Length > 500)
            throw new InvalidDescriptionException(500);

        if (stockQuantity < 0)
            throw new InvalidStockQuantityException(stockQuantity);

        if (categoryId == Guid.Empty)
            throw new RequiredFieldException(nameof(categoryId));

        return new Product(
            Guid.NewGuid(),
            name,
            description,
            price,
            stockQuantity,
            imageUrl,
            categoryId
        );
    }

    public void Update(
        Name name,
        string description,
        Money price,
        string? imageUrl,
        Guid categoryId
    )
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidDescriptionException();

        if (description.Length > 500)
            throw new InvalidDescriptionException(500);

        if (categoryId == Guid.Empty)
            throw new RequiredFieldException(nameof(categoryId));

        Name = name;
        Description = description;
        Price = price;
        ImageUrl = imageUrl;
        CategoryId = categoryId;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidStockQuantityException(quantity);

        StockQuantity += quantity;
        IsAvailable = StockQuantity > 0;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidStockQuantityException(quantity);

        if (quantity > StockQuantity)
            throw new InsufficientStockException(StockQuantity, quantity);

        StockQuantity -= quantity;
        IsAvailable = StockQuantity > 0;
    }

    public void UpdatePrice(Money price)
    {
        if (price is null)
            throw new ArgumentNullException(nameof(price));

        Price = price;
    }
}
