using BuildingBlocks.User;
using BuildingBlocks.Exceptions;

namespace OrderService.Features.Order.GetOrderById;

public class GetOrderByIdHandler(IDocumentSession session, IUserContext userContext) 
        : IRequestHandler<GetOrderByIdCommand, Models.Order>
{
    public async Task<Models.Order> Handle(GetOrderByIdCommand request, CancellationToken cancellationToken)
    {
        var order = await session.Query<Models.Order>()
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken) 
            ?? throw new NotFoundException(nameof(Models.Order), request.Id.ToString());
        
        var currentUser = userContext.GetCurrentUser();
        if(order.UserId != currentUser.Id)
            throw new ForbiddenException("You are not allowed to access this order.");

        return order;
    }
}
