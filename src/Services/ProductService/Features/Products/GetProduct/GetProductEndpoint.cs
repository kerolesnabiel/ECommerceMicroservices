using Microsoft.AspNetCore.Mvc;

namespace ProductService.Features.Products.GetProduct;

public class GetProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products/{id}",
        async ([FromRoute] Guid id, ISender sender) =>
        {
            var product = await sender.Send(new GetProductQuery(id));
            return Results.Ok(product);
        })
        .WithName("GetProduct")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
