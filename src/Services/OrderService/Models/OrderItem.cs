namespace OrderService.Models;

public class OrderItem
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public string Image { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
