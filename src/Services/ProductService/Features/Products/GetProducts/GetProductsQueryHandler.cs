using ProductService.Models;

namespace ProductService.Features.Products.GetProducts;

public class GetProductsQueryHandler(IDocumentSession session) 
        : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        request.Search ??= "";

        int skip = (request.PageNumber - 1) * request.PageSize;
        var products = await session.Query<Product>()
            .Where(p => 
            (request.Category == null || p.Category == request.Category) &&
            (request.MinPrice == null || p.Price >= request.MinPrice) &&
            (request.MaxPrice == null || p.Price <= request.MaxPrice) &&
            ((  p.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(request.Search, StringComparison.OrdinalIgnoreCase))))
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return products;
    }
}
