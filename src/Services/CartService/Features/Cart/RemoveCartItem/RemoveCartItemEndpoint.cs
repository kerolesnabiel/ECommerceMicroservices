using Microsoft.AspNetCore.Mvc;

namespace CartService.Features.Cart.RemoveCartItem;

public class RemoveCartItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/cart/items/{id}",
        async ([FromRoute] Guid id, ISender sender) =>
        {
            await sender.Send(new RemoveCartItemCommand(id));
            return Results.NoContent();
        })
        .WithName("RemoveCartItem")
        .Produces(StatusCodes.Status204NoContent)
        .RequireAuthorization();
    }
}
