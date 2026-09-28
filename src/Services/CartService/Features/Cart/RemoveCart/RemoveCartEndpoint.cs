namespace CartService.Features.Cart.RemoveCart;

public class RemoveCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/cart", async ( ISender sender) =>
        {
            await sender.Send(new RemoveCartCommand());
            return Results.NoContent();
        })
        .WithName("RemoveCart")
        .Produces(StatusCodes.Status204NoContent)
        .RequireAuthorization();
    }
}
