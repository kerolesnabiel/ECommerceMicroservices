using CartService.Models;

namespace CartService.DTOs;

public static class MappingProfile
{
    public static void Configure()
    {
        TypeAdapterConfig<Cart, CartDto>.NewConfig()
            .Map(dest => dest.Items, src => src.Items)
            .AfterMapping((src, dest) =>
            {
                dest.TotalPrice = dest.Items.Sum(i => i.TotalPrice);
                dest.ItemsCount = dest.Items.Sum(i => i.Quantity);
            });

        TypeAdapterConfig<CartItem, CartItemDto>.NewConfig()
            .Map(dest => dest.TotalPrice, src => src.Price * src.Quantity);
    }
}
