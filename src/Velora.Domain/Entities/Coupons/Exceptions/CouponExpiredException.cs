using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Coupons.Exceptions;

public sealed class CouponExpiredException : DomainException
{
    public CouponExpiredException()
        : base("Coupon has expired.") { }
}
