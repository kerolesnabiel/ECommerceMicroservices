namespace OrderService.Features.Order.CancelOrder;

public class CancelOrderCommand(Guid id) : IRequest
{
    public Guid Id { get; } = id;
}
