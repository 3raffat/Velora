using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Coupons.Exceptions;

public sealed class CouponExpiredException : DomainException
{
    public CouponExpiredException()
        : base("Coupon has expired.") { }
}
