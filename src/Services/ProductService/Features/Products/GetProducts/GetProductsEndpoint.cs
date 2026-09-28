using Microsoft.AspNetCore.Mvc;

namespace ProductService.Features.Products.GetProducts;

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products",
        async ( ISender sender,
            [FromQuery] string? category,
            [FromQuery] string? search,
            [FromQuery] int? minPrice,
            [FromQuery] int? maxPrice,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
            ) =>
        {
            var query = new GetProductsQuery(search, category, minPrice, maxPrice, pageNumber, pageSize);
            var products = await sender.Send(query);
            return Results.Ok(products);
        })
        .WithName("GetProducts")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
