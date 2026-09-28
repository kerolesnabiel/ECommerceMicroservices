namespace CartService.Models;

public class CartItem
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public string Image { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
