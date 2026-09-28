using UserService.Domain.Entities;

namespace UserService.Application.Addresses.DTOs;

public class AddressDto
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
}
