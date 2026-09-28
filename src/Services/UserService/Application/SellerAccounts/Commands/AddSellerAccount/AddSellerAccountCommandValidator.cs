using FluentValidation;

namespace UserService.Application.SellerAccounts.Commands.AddSellerAccount;

public class AddSellerAccountCommandValidator : AbstractValidator<AddSellerAccountCommand>

{
    public AddSellerAccountCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(x => x.MiddleName)
            .MaximumLength(50);

        RuleFor(x => x.CountryOfCitizenship)
            .NotEmpty()
            .Length(2, 100);

        RuleFor(x => x.CountryOfBirth)
            .NotEmpty()
            .Length(2, 100);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThan(DateTime.Now);

        RuleFor(x => x.StoreName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.StoreDescription)
            .MaximumLength(500);
    }
}
