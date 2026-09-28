namespace CartService.Features.Cart.Checkout;

public class CheckoutCommand : IRequest
{
    public string CardToken { get; set; } = default!;

    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Address1 { get; set; } = default!;
    public string? Address2 { get; set; }
}
