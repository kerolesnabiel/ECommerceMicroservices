namespace CartService.Features.Cart.AddItemToCart;

public class AddItemToCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/cart/items",
        async (AddItemToCartCommand request, ISender sender) =>
        {
            await sender.Send(request);
            return Results.Created("api/cart", null);
        })
        .WithName("AddToCart")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
    }
}
