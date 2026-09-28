namespace CartService.DTOs;

public class CartItemDto
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public string Image { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime AddedAt { get; set; }
    public decimal TotalPrice { get; set; }
}
