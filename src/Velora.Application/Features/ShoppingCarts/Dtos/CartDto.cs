using Velora.Domain.Entities.ShoppingCart.Enums;

namespace Velora.Application.Features.ShoppingCarts.Dtos;

public record CartDto(
    Guid Id,
    Guid CustomerId,
    CartStatus Status,
    decimal TotalAmount,
    IEnumerable<CartItemDto> Items
);
