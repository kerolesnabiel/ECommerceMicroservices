using MediatR;

namespace UserService.Application.SellerAccounts.Commands.AddSellerAccount;

public class AddSellerAccountCommand : IRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string CountryOfCitizenship { get; set; } = default!;
    public string CountryOfBirth { get; set; } = default!;
    public DateTime DateOfBirth { get; set; } = default!;
    public string StoreName { get; set; } = default!;
    public string StoreDescription { get; set; } = string.Empty;
}
