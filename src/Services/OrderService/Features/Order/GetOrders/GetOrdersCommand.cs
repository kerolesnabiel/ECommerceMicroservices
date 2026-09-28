using OrderService.DTOs;

namespace OrderService.Features.Order.GetOrders;

public class GetOrdersCommand : IRequest<IEnumerable<OrderMiniDto>>
{
}
