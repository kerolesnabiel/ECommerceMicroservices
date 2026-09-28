namespace CartService.DTOs;

public class CartDto 
{
    public Guid Id { get; set; }
    public ICollection<CartItemDto> Items { get; set; } = [];
    public int ItemsCount { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime UpdatedAt { get; set; }
}
