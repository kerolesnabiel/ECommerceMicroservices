namespace UserService.Domain.Entities;

public class SellerAccount
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string CountryOfCitizenship { get; set; } = default!;
    public string CountryOfBirth { get; set; } = default!;
    public DateTime DateOfBirth { get; set; } = default!;

    public string StoreName { get; set; } = default!;
    public string StoreDescription { get; set; } = string.Empty;
    //public bool IsVerified { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
