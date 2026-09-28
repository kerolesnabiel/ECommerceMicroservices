namespace UserService.Domain.Entities;

public class Address
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Address1 { get; set; } = default!;
    public string? Address2 { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}
