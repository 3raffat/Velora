namespace OrderService.Application.Features.ShoppingCarts.Exceptions;

public sealed class CouponExpiredException : Exception
{
    public CouponExpiredException()
        : base("The coupon has expired.") { }
}
