using CartService.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CartService.Data;

public class CachedCartRepository(IDistributedCache cache, 
        ICartRepository cartRepository) : ICartRepository
{
    public async Task AddCartAsync(Cart cart, CancellationToken cancellationToken = default)
    {
       await cartRepository.AddCartAsync(cart, cancellationToken);
       await cache.SetStringAsync(cart.Id.ToString(), 
           JsonSerializer.Serialize(cart), cancellationToken);
    }

    public async Task<Cart?> GetByIdAsync(Guid id)
    {
        var cartJson = await cache.GetStringAsync(id.ToString());
        if (!string.IsNullOrEmpty(cartJson))
            return JsonSerializer.Deserialize<Cart>(cartJson);

        var cart = await cartRepository.GetByIdAsync(id);
        if(cart != null)
            cache.SetString(cart.Id.ToString(), JsonSerializer.Serialize(cart));
        return cart;
    }
}
