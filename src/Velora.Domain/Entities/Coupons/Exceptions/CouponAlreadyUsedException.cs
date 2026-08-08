using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Coupons.Exceptions;

public sealed class CouponAlreadyUsedException : DomainException
{
    public CouponAlreadyUsedException()
        : base("Coupon has already been used.") { }
}
