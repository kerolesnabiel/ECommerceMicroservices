
namespace CartService.Features.Cart.GetCart;

public class GetCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/cart", async (ISender sender) =>
        {
            var cart = await sender.Send(new GetCartQuery());
            return Results.Ok(cart);
        })
        .WithName("GetCart")
        .Produces(StatusCodes.Status200OK)
        .RequireAuthorization();
    }
}
