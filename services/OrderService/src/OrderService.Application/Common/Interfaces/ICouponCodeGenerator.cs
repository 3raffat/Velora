using OrderService.Domain.Entities.Coupons.Enums;

namespace OrderService.Application.Common.Interfaces;

public interface ICouponCodeGenerator
{
    string Generate(CouponType type, int length = 6);
}
