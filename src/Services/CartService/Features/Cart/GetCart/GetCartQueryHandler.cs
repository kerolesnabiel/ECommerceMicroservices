using CartService.Data;
using CartService.DTOs;
using BuildingBlocks.User;

namespace CartService.Features.Cart.GetCart;

public class GetCartQueryHandler(
    ICartRepository cartRepository, IUserContext userContext) 
        : IRequestHandler<GetCartQuery, CartDto>
{
    public async Task<CartDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var cart = await cartRepository.GetByIdAsync(currentUser.Id)
            ?? new() { Id = currentUser.Id };

        return cart.Adapt<CartDto>();
    }
}
