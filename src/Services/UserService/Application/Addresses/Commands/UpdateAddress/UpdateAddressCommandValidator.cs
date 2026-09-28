using FluentValidation;

namespace UserService.Application.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressCommandValidator()
    {
        RuleFor(a => a.FullName)
            .NotEmpty()
            .MaximumLength(100)
            .When(a => a.FullName != null);

        RuleFor(a => a.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must be in a valid international format.")
            .When(a => a.PhoneNumber != null);

        RuleFor(a => a.Country)
            .NotEmpty()
            .MaximumLength(50)
            .When(a => a.Country != null);

        RuleFor(a => a.City)
            .NotEmpty()
            .MaximumLength(50)
            .When(a => a.City != null);

        RuleFor(a => a.State)
            .NotEmpty()
            .MaximumLength(50)
            .When(a => a.State != null);

        RuleFor(a => a.ZipCode)
            .NotEmpty()
            .Matches(@"^\d{5}(-\d{4})?$")
            .WithMessage("Zip code must be in a valid format (e.g., 12345 or 12345-6789).")
            .When(a => a.ZipCode != null);

        RuleFor(a => a.Address1)
            .NotEmpty()
            .MaximumLength(200)
            .When(a => a.Address1 != null);

        RuleFor(a => a.Address2)
            .MaximumLength(200);
    }
}
