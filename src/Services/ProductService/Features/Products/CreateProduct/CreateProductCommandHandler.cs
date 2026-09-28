using ProductService.Models;
using BuildingBlocks.User;

namespace ProductService.Features.Products.CreateProduct;

public class CreateProductCommandHandler(
    IDocumentSession session, IUserContext userContext
    ) : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (string.IsNullOrEmpty(currentUser.SellerId))
            throw new BadHttpRequestException("The user does not have a seller account");

        bool validId = Guid.TryParse(currentUser.SellerId, out Guid sellerId);
        if (!validId) throw new BadHttpRequestException("Invalid seller Id");

        Product product = request.Adapt<Product>();
        product.SellerId = sellerId;

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}