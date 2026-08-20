using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;
using OrderService.Domain.Entities.Customers;
using OrderService.Domain.Entities.Customers.Exceptions;
using OrderService.Domain.Entities.Products.Exceptions;
using OrderService.Domain.Entities.Products.ValueObjects;

namespace OrderService.Domain.Entities.Products;

public sealed class Feedback : SoftDeletableEntity
{
    public Rating Rating { get; private set; } = null!;
    public string? Comment { get; private set; }

    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    private Feedback() { }

    private Feedback(Guid id, Rating rating, string? comment, Guid customerId, Guid productId)
        : base(id)
    {
        Rating = rating;
        Comment = comment?.Trim();
        CustomerId = customerId;
        ProductId = productId;
    }

    public static Feedback Create(Rating rating, string? comment, Guid customerId, Guid productId)
    {
        if (rating is null)
            throw new ArgumentNullException(nameof(rating));

        if (customerId == Guid.Empty)
            throw new RequiredFieldException(nameof(customerId));

        if (productId == Guid.Empty)
            throw new RequiredFieldException(nameof(productId));

        if (comment is not null && comment.Length > 500)
            throw new InvalidCommentException(500);

        return new Feedback(Guid.NewGuid(), rating, comment, customerId, productId);
    }
}
