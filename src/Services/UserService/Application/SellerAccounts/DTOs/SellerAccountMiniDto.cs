namespace UserService.Application.SellerAccounts.DTOs;

public class SellerAccountMiniDto
{
    public Guid Id { get; set; }
    public string StoreName { get; set; } = default!;
    public string? StoreDescription { get; set; }
}
