
using Microsoft.AspNetCore.Mvc;

namespace CartService.Features.Cart.UpdateCartItem;

public class UpdateCartItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/cart/items/{id}",
        async ([FromRoute] Guid id, UpdateCartItemCommand request, ISender sender) =>
        {
            request.ProductId = id;
            await sender.Send(request);
            return Results.NoContent();
        })
        .WithName("UpdateCartItem")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
    }
}
