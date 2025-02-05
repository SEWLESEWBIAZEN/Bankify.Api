using Bankify.Domain.Models.Users;
using FluentValidation;

namespace Bankify.Domain.Validators
{
    public class UserDataValidator:AbstractValidator<BUser>
    {
        public UserDataValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
        }
    }
}
