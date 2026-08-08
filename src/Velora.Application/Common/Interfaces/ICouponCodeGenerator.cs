using Velora.Domain.Entities.Coupons.Enums;

namespace Velora.Application.Common.Interfaces;

public interface ICouponCodeGenerator
{
    string Generate(CouponType type, int length = 6);
}
