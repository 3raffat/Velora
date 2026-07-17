using Velora.Domain.Common;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Orders;

namespace Velora.Domain.Entities.Products;

public class Product : BaseEntity
{
    public Name Name { get; private set; } = null!;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
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
        decimal price,
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
        decimal price,
        int stockQuantity,
        string? imageUrl,
        Guid categoryId
    )
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");

        if (stockQuantity < 0)
            throw new ArgumentOutOfRangeException(
                nameof(stockQuantity),
                "Stock quantity cannot be negative."
            );

        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category Id is required.", nameof(categoryId));

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

    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Stock quantity cannot be negative."
            );

        StockQuantity = quantity;
        IsAvailable = quantity > 0;
    }

    public void UpdatePrice(decimal price)
    {
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");

        Price = price;
    }
}
