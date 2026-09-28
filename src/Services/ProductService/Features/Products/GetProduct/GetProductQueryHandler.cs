using BuildingBlocks.Exceptions;
using ProductService.Models;

namespace ProductService.Features.Products.GetProduct;

public class GetProductQueryHandler(IDocumentSession session) 
        : IRequestHandler<GetProductQuery, Product>
{
    public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id.ToString());

        return product;
    }
}
