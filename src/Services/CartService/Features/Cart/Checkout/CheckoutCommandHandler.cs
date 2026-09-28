using BuildingBlocks.Events;
using BuildingBlocks.User;
using CartService.Data;
using MassTransit;
using static PaymentService.PaymentServiceProto;

namespace CartService.Features.Cart.Checkout;

public class CheckoutCommandHandler(
    PaymentServiceProtoClient paymentService,
    IPublishEndpoint publishEndpoint,
    ICartRepository cartRepository,
    IUserContext userContext)
        : IRequestHandler<CheckoutCommand>
{
    public async Task Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        var userId = userContext.GetCurrentUser().Id;
        var cart = await cartRepository.GetByIdAsync(userId);

        if (cart == null || cart.Items.Count == 0)
            throw new BadHttpRequestException("Your cart is empty");

        decimal total = cart.Items.Sum(i => i.Price * i.Quantity);
        long amount = (long)total * 100;

        var response = await paymentService.ChargeAsync(new()
        {
            UserId = userId.ToString(),
            CardToken = request.CardToken,
            Amount = amount,
            Currency = "usd"
        }, cancellationToken: cancellationToken) ?? throw new BadHttpRequestException("Payment failed");

        var checkoutEvent = request.Adapt<CartCheckoutEvent>();
        checkoutEvent.UserId = userId;
        checkoutEvent.TotalPrice = total;
        checkoutEvent.TransactionId = response.TransactionId;
        checkoutEvent.Items = cart.Items.Adapt<List<CheckoutItem>>();

        await publishEndpoint.Publish(checkoutEvent, cancellationToken);
        cart.Items.Clear();
        await cartRepository.AddCartAsync(cart, cancellationToken);
    } 
}