namespace CartService.Data;

public interface ICartRepository
{
    Task<Models.Cart?> GetByIdAsync(Guid id);
    Task AddCartAsync(Models.Cart cart, CancellationToken cancellationToken = default);
}
