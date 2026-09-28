namespace BuildingBlocks.Events;

public class CartCheckoutEvent
{
    public Guid UserId { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;
    public List<CheckoutItem> Items { get; set; } = [];

    // Shipping and BillingAddress
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Address1 { get; set; } = default!;
    public string? Address2 { get; set; }

    // Payment
    public string TransactionId { get; set; } = default!;
}

public class CheckoutItem
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public string Image { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}