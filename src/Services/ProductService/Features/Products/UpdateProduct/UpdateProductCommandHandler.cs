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
        if (string.IsNullOrEmpty(currentUser.SellerId))
            throw new BadHttpRequestException("The user does not have a seller account");

        bool validId = Guid.TryParse(currentUser.SellerId, out Guid sellerId);
        if (!validId) throw new BadHttpRequestException("Invalid seller Id");

        var product = await session.LoadAsync<Product>(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id.ToString());

        if (product.SellerId != sellerId) 
            throw new ForbiddenException();

        var config = new TypeAdapterConfig();
        config.ForType<UpdateProductCommand, Product>().IgnoreNullValues(true);
        product = request.Adapt(product, config);

        product.UpdatedAt = DateTime.UtcNow;
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
    }
}
