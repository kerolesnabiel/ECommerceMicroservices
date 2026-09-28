using CartService.Data;
using BuildingBlocks.User;

namespace CartService.Features.Cart.RemoveCart;

public class RemoveCartCommandHandler(
    ICartRepository cartRepository, IUserContext userContext) 
        : IRequestHandler<RemoveCartCommand>
{
    public async Task Handle(RemoveCartCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var cart = await cartRepository.GetByIdAsync(currentUser.Id);

        if (cart == null || cart.Items.Count == 0)
            return;

        cart.Items.Clear();
        cart.UpdatedAt = DateTime.UtcNow;
        await cartRepository.AddCartAsync(cart, cancellationToken);
    }
}
