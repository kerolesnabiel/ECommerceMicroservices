using OrderService.Constants;

namespace OrderService.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;
    public List<OrderItem> Items { get; set; } = [];
    public OrderAddress Address { get; set; } = default!;
    public string TransactionId { get; set; } = default!;
    public string Status { get; set; } = OrderStatus.Processing;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
