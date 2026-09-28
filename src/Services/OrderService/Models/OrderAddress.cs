namespace OrderService.Models;

public class OrderAddress
{
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Address1 { get; set; } = default!;
    public string? Address2 { get; set; }
}
