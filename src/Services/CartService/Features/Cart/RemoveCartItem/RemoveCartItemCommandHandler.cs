using CartService.Data;
using BuildingBlocks.User;

namespace CartService.Features.Cart.RemoveCartItem;

public class RemoveCartItemCommandHandler(
    ICartRepository cartRepository, IUserContext userContext) 
        : IRequestHandler<RemoveCartItemCommand>
{
    public async Task Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var cart = await cartRepository.GetByIdAsync(currentUser.Id);
        if (cart == null) return;

        var item = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);

        if (item == null) return;

        cart.Items.Remove(item);
        await cartRepository.AddCartAsync(cart, cancellationToken);
    }
}
