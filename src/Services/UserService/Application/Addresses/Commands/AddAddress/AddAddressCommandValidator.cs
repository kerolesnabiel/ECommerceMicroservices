using FluentValidation;

namespace UserService.Application.Addresses.Commands.AddAddress;

public class AddAddressCommandValidator : AbstractValidator<AddAddressCommand>
{
    public AddAddressCommandValidator()
    {
        RuleFor(a => a.FullName)
            .NotEmpty()
            .MaximumLength(100);
            
        RuleFor(a => a.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must be in a valid international format.");

        RuleFor(a => a.Country)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(a => a.City)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(a => a.State)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(a => a.ZipCode)
            .NotEmpty()
            .Matches(@"^\d{5}(-\d{4})?$")
            .WithMessage("Zip code must be in a valid format (e.g., 12345 or 12345-6789).");

        RuleFor(a => a.Address1)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(a => a.Address2)
            .MaximumLength(200);
    }
}

