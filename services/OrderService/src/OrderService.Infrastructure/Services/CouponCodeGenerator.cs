using System.Security.Cryptography;
using OrderService.Application.Common.Interfaces;
using OrderService.Domain.Entities.Coupons.Enums;

namespace OrderService.Infrastructure.Services;

public sealed class CouponCodeGenerator : ICouponCodeGenerator
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string Generate(CouponType type, int length = 6)
    {
        var prefix = type.ToString().ToUpperInvariant();

        var random = RandomNumberGenerator.GetString(chars, length);

        return $"{prefix}-{random}";
    }
}
