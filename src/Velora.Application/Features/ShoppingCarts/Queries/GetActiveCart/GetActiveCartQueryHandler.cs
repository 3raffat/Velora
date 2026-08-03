using MediatR;
using Microsoft.EntityFrameworkCore;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.ShoppingCarts.Dtos;
using Velora.Application.Features.ShoppingCarts.Mapper;
using Velora.Domain.Entities.ShoppingCart.Enums;

namespace Velora.Application.Features.ShoppingCarts.Queries.GetActiveCart;

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
