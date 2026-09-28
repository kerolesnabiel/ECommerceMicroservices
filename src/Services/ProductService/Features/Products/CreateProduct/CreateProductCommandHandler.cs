using ProductService.Models;
using BuildingBlocks.User;

namespace ProductService.Features.Products.CreateProduct;

public class CreateProductCommandHandler(
    IDocumentSession session,
    IUserContext userContext
) : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        Product product = request.Adapt<Product>();
        product.SellerId = currentUser.Id;

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}