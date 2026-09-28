namespace CartService.Features.Cart.RemoveCartItem;

public class RemoveCartItemCommand(Guid id) : IRequest
{
    public Guid ProductId { get; set; } = id;
}
