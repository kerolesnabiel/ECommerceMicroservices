namespace OrderService.DTOs;

public class OrderMiniDto
{
    public Guid Id { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;
    public string TransactionId { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = default!;
    public DateTime? UpdatedAt { get; set; }
}
