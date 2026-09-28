using FluentValidation;

namespace UserService.Application.SellerAccounts.Commands.UpdateSellerAccount;

public class UpdateSellerAccountCommandValidator : AbstractValidator<UpdateSellerAccountCommand>
{
    public UpdateSellerAccountCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(2, 50)
            .When(x => x.FirstName != null);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(2, 50)
            .When(x => x.LastName != null);

        RuleFor(x => x.MiddleName)
            .MaximumLength(50);

        RuleFor(x => x.CountryOfCitizenship)
            .NotEmpty()
            .Length(2, 100).
            When(x => x.CountryOfCitizenship != null);

        RuleFor(x => x.CountryOfBirth)
            .NotEmpty()
            .Length(2, 100)
            .When(x => x.CountryOfBirth != null);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThan(DateTime.Now)
            .When(x => x.DateOfBirth != null);

        RuleFor(x => x.StoreName)
            .NotEmpty()
            .Length(3, 100)
            .When(x => x.StoreName != null);

        RuleFor(x => x.StoreDescription)
            .MaximumLength(500)
            .When(x => x.StoreDescription != null);
    }
}
