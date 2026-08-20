using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.ShoppingCarts.Dtos;
using OrderService.Application.Features.ShoppingCarts.Mapper;
using OrderService.Domain.Entities.ShoppingCart.Enums;

namespace OrderService.Application.Features.ShoppingCarts.Queries.GetActiveCart;

public sealed class GetActiveCartQueryHandler(IVeloraContext _context)
    : IRequestHandler<GetActiveCartQuery, CartDto?>
{
    public async Task<CartDto?> Handle(GetActiveCartQuery request, CancellationToken ct)
    {
        var cart = await _context
            .Carts.Include(c => c.CartItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.CustomerId == request.CustomerId && c.Status == CartStatus.Active,
                ct
            );

        return cart?.ToDto();
    }
}
