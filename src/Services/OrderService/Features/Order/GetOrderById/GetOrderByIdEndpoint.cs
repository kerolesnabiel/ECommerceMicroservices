
namespace OrderService.Features.Order.GetOrderById;

public class GetOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders/{id}", async (Guid id, ISender sender) =>
        {
            var order = await sender.Send(new GetOrderByIdCommand(id));
            return Results.Ok(order);
        })
        .WithName("GetOrderById")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
