namespace OrderService.Features.Order.GetOrderById;

public class GetOrderByIdCommand(Guid id) : IRequest<Models.Order>
{
    public Guid Id { get; set; } = id;
}
