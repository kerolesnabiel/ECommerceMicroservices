using CartService.Models;

namespace CartService.Data;

public class CartRepository(IDocumentSession session) : ICartRepository
{
    public async Task<Cart?> GetByIdAsync(Guid id)
    {
        return await session.LoadAsync<Cart>(id);
    }

    public async Task AddCartAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        session.Store(cart);
        await session.SaveChangesAsync(cancellationToken);
    }
}
