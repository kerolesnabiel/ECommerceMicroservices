using CartService.Data;
using ProductService;
using BuildingBlocks.Exceptions;
using BuildingBlocks.User;

namespace CartService.Features.Cart.UpdateCartItem;

public class UpdateCartItemCommandHandler(
    ICartRepository cartRepository, IUserContext userContext,
    ProductServiceProto.ProductServiceProtoClient productService) 
        : IRequestHandler<UpdateCartItemCommand>
{
    public async Task Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var cart = await cartRepository.GetByIdAsync(currentUser.Id)
            ?? new() { Id = currentUser.Id };

        var item = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId)
            ?? throw new NotFoundException("Item not found in cart.");

        if(item.Quantity == request.Quantity)
            return;

        if (request.Quantity > item.Quantity)
        {
            var product = await productService.GetProductAsync(
                new() { Id = item.ProductId.ToString() }, cancellationToken: cancellationToken);

            if (product.StockQuantity < request.Quantity)
                throw new InsufficientStockException(request.Quantity, product.StockQuantity);
        }

        item.Quantity = request.Quantity;
        await cartRepository.AddCartAsync(cart, cancellationToken);
    }
}
