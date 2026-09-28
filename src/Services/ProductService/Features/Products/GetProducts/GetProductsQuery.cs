using ProductService.Models;

namespace ProductService.Features.Products.GetProducts;

public class GetProductsQuery(string? search, string? category, 
        int? minPrice, int? maxPrice, int pageNumber, int pageSize) 
            : IRequest<IEnumerable<Product>>
{
    public string? Search { get; set; } = search;
    public string? Category { get; set; } = category;
    public int? MinPrice { get; set; } = minPrice;
    public int? MaxPrice { get; set; } = maxPrice;
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
}
