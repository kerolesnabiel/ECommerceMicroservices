namespace UserService.Application.SellerAccounts.DTOs;

public class SellerAccountDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string CountryOfCitizenship { get; set; } = default!;
    public string CountryOfBirth { get; set; } = default!;
    public DateTime DateOfBirth { get; set; } = default!;
    public string StoreName { get; set; } = default!;
    public string? StoreDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
