namespace CartService.Features.Cart.AddItemToCart;

public class AddItemToCartCommand : IRequest
{
    public string ProductId { get; set; } = default!;
    public int Quantity { get; set; }
}
