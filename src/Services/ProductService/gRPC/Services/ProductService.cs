using Grpc.Core;
using ProductService.Models;

namespace ProductService.gRPC.Services;

public class ProductService(IDocumentSession session) 
        : ProductServiceProto.ProductServiceProtoBase
{
    public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        bool isValid = Guid.TryParse(request.Id, out Guid id);
        if(!isValid)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid product ID"));

        var product = await session.LoadAsync<Product>(id)
            ?? throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));

        if(product.StockQuantity <= 0)
            throw new RpcException(new Status(StatusCode.FailedPrecondition, "Product out of stock"));

        var res = product.Adapt<GetProductResponse>();
        res.Image = product.Images.FirstOrDefault() ?? "";
        return res;
    }
}
