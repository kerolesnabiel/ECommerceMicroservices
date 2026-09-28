using MediatR;

namespace UserService.Application.SellerAccounts.Commands.UpdateSellerAccount;

public class UpdateSellerAccountCommand : IRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? CountryOfCitizenship { get; set; }
    public string? CountryOfBirth { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? StoreName { get; set; }
    public string? StoreDescription { get; set; }
}
