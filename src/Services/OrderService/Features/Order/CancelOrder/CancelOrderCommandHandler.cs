using BuildingBlocks.Exceptions;
using BuildingBlocks.User;
using OrderService.Constants;

namespace OrderService.Features.Order.CancelOrder;

public class CancelOrderCommandHandler(IDocumentSession session, IUserContext userContext) : IRequestHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await session.Query<Models.Order>()
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken) 
            ?? throw new NotFoundException(nameof(Models.Order), request.Id.ToString());

        var currentUser = userContext.GetCurrentUser();
        if (order.UserId != currentUser.Id)
            throw new ForbiddenException("You are not allowed to access this order.");

        if (order.Status != OrderStatus.Processing)
            throw new BadHttpRequestException("You can only cancel orders that are still processing.");
        
        order.Status = OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;
        session.Update(order);
        await session.SaveChangesAsync(cancellationToken);
    }
}
