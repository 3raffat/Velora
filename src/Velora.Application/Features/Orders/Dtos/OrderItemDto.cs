namespace Velora.Application.Features.Orders.Dtos;

public sealed record OrderItemDto(
    Guid Id,
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal Discount,
    decimal TotalPrice
);
