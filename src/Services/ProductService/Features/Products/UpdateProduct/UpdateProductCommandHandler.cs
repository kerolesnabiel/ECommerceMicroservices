using BuildingBlocks.Exceptions;
using ProductService.Models;
using BuildingBlocks.User;

namespace ProductService.Features.Products.UpdateProduct;

public class UpdateProductCommandHandler(
    IDocumentSession session, IUserContext userContext) 
        : IRequestHandler<UpdateProductCommand>
{
    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        var product = await session.LoadAsync<Product>(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id.ToString());

        if (product.SellerId != currentUser.Id) 
            throw new ForbiddenException();

        var config = new TypeAdapterConfig();
        config.ForType<UpdateProductCommand, Product>().IgnoreNullValues(true);
        product = request.Adapt(product, config);

        product.UpdatedAt = DateTime.UtcNow;
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
    }
}
