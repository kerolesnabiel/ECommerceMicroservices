using Microsoft.AspNetCore.Mvc;

namespace ProductService.Features.Products.UpdateProduct;

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/products/{id}",
        async ([FromRoute] Guid id, UpdateProductCommand request, ISender sender) =>
        {
            request.Id = id;
            await sender.Send(request);
            return Results.NoContent();
        })
        .WithName("UpdateProduct")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
    }
}
