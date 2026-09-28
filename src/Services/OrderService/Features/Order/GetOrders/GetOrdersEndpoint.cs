
namespace OrderService.Features.Order.GetOrders;

public class GetOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders", async (ISender sender) =>
        {
            var orders = await sender.Send(new GetOrdersCommand());
            return Results.Ok(orders);  
        })
        .WithName("GetOrders")
        .Produces(StatusCodes.Status200OK)
        .RequireAuthorization();
    }
}
