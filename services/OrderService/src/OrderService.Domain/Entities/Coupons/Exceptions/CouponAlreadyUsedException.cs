using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Coupons.Exceptions;

public sealed class CouponAlreadyUsedException : DomainException
{
    public CouponAlreadyUsedException()
        : base("Coupon has already been used.") { }
}
