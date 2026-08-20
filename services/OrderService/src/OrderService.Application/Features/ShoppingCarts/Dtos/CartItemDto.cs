namespace OrderService.Application.Features.ShoppingCarts.Dtos;

public record CartItemDto(
    Guid Id,
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal Discount,
    decimal TotalPrice
);
