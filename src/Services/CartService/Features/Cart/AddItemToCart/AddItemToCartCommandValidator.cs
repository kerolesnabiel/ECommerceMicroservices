namespace CartService.Features.Cart.AddItemToCart;

public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .Must(BeAValidGuid)
            .WithMessage("Invalid Product Id."); ;

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1)
            .NotEmpty();
    }

    private bool BeAValidGuid(string productId) => 
         Guid.TryParse(productId, out Guid id);

}
