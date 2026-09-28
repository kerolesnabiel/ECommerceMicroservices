namespace CartService.Features.Cart.Checkout;

public class CheckoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/cart/checkout",
        async (CheckoutCommand request, ISender sender) =>
        {
            await sender.Send(request);
            return Results.Ok();
        })
        .WithName("Checkout")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
    }
}
