namespace ProductService.Features.Products.GetProducts;

public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    private readonly List<int> allowed = [10, 20, 30, 50, 100];
    public GetProductsQueryValidator()
    {
        RuleFor(x => x.PageSize)
            .Must(allowed.Contains)
            .WithMessage("Page size must be in [10, 20, 30, 50, 100]");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.MinPrice)
            .GreaterThan(0)
            .When(x => x.MinPrice != null);
        
        RuleFor(x => x.MaxPrice)
            .GreaterThan(x => x.MinPrice?? 0)
            .When(x => x.MaxPrice != null);

        RuleFor(x => x.Search)
            .NotEmpty()
            .When(x => x.Search != null);
        
        RuleFor(x => x.Category)
            .NotEmpty()
            .When(x => x.Category != null);
    }
}
