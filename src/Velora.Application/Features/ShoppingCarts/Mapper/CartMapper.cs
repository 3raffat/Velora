using Velora.Application.Features.ShoppingCarts.Dtos;
using Velora.Domain.Entities.ShoppingCart;

namespace Velora.Application.Features.ShoppingCarts.Mapper;

public static class CartMapper
{
    public static CartItemDto ToDto(this CartItem item)
    {
        return new CartItemDto(
            item.Id,
            item.ProductId,
            item.Quantity,
            item.UnitPrice.Amount,
            item.Discount,
            item.TotalPrice
        );
    }

    public static IEnumerable<CartItemDto> ToDtos(this IEnumerable<CartItem> items)
    {
        return items.Select(i => i.ToDto()).ToList();
    }

    public static CartDto ToDto(this Cart cart)
    {
        var items = cart.CartItems.ToDtos();
        var totalAmount = items.Sum(i => i.TotalPrice);

        return new CartDto(cart.Id, cart.CustomerId, cart.Status, totalAmount, items);
    }
}
