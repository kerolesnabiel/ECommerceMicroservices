namespace ProductService.Features.Products.CreateProduct;

public class CreateProductEndpoint() : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/products",
        async (CreateProductCommand request, ISender sender) =>
        {
            var productId = await sender.Send(request);

            return Results.Created($"/api/products/{productId}", null);
        })
        .WithName("CreateProduct")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
    }
}
