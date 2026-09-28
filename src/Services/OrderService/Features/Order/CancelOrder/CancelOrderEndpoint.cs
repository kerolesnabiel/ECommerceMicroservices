namespace OrderService.Features.Order.CancelOrder;

public class CancelOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/orders/{id}/cancel", async (Guid id, ISender sender) =>
        {
            await sender.Send(new CancelOrderCommand(id));
            return Results.NoContent();
        })
        .WithName("CancelOrder")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequireAuthorization();
    }
}
