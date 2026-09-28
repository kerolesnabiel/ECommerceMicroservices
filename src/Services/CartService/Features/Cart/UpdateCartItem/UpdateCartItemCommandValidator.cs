namespace CartService.Features.Cart.UpdateCartItem;

public class UpdateCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand>
{
    public UpdateCartItemCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1)
            .NotEmpty();
    }
}
