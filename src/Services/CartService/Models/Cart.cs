namespace CartService.Models;

public class Cart
{
    public Cart() {}
    public Cart(Guid userId) => Id = userId;

    public Guid Id { get; set; }
    public ICollection<CartItem> Items { get; set; } = [];
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
