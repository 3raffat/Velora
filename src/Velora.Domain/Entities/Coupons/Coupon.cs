using Velora.Domain.Common;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Coupons.Enums;
using Velora.Domain.Entities.Coupons.Exceptions;

namespace Velora.Domain.Entities.Coupons;

public class Coupon : AuditableEntity
{
    public Guid CustomerId { get; private set; }

    public string Code { get; private set; } = string.Empty;

    public Money Discount { get; private set; } = null!;

    public DateTime ExpiryDate { get; private set; }
    public CouponType Type { get; private set; }

    public bool IsUsed { get; private set; }

    private Coupon() { }

    private Coupon(
        Guid customerId,
        string code,
        Money discount,
        DateTime expiryDate,
        CouponType type
    )
    {
        CustomerId = customerId;
        Code = code;
        Discount = discount;
        ExpiryDate = expiryDate;
        Type = type;
    }

    public static Coupon Create(
        Guid customerId,
        string code,
        decimal discountPercentage,
        DateTime expiresAt,
        CouponType type
    )
    {
        if (discountPercentage <= 0 || discountPercentage > 100)
            throw new ArgumentOutOfRangeException(nameof(discountPercentage));

        if (expiresAt <= DateTime.UtcNow)
            throw new CouponExpiredException();

        return new Coupon(customerId, code, Money.Create(discountPercentage), expiresAt, type);
    }

    public void Use()
    {
        if (IsUsed)
            throw new CouponAlreadyUsedException();

        if (ExpiryDate <= DateTime.UtcNow)
            throw new CouponExpiredException();

        IsUsed = true;
    }
}
