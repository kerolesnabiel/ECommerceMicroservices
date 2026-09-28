using BuildingBlocks.User;
using OrderService.DTOs;

namespace OrderService.Features.Order.GetOrders;

public class GetOrdersCommandHandler(IDocumentSession session, IUserContext userContext) 
        : IRequestHandler<GetOrdersCommand, IEnumerable<OrderMiniDto>>
{
    public async Task<IEnumerable<OrderMiniDto>> Handle(GetOrdersCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var orders = await session.Query<Models.Order>()
            .Where(o => o.UserId == currentUser.Id)
            .ToListAsync(cancellationToken);
        return orders.Adapt<IEnumerable<OrderMiniDto>>();
    }
}
