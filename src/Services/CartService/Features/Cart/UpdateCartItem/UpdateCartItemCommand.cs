namespace CartService.Features.Cart.UpdateCartItem;

public class UpdateCartItemCommand : IRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
