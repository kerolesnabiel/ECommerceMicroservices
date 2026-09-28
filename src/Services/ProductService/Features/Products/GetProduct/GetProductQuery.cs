using ProductService.Models;

namespace ProductService.Features.Products.GetProduct;

public class GetProductQuery(Guid id) : IRequest<Product>
{
    public Guid Id { get; set; } = id;
}
