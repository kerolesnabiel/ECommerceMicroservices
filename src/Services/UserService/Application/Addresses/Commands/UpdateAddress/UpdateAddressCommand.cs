using MediatR;

namespace UserService.Application.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommand : IRequest
{
    public Guid Id { get; set; } = default!;
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
}
