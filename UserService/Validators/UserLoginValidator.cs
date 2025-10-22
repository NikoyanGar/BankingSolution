using FluentValidation;
using UserService.Models;

namespace UserService.Validators
{
    public class UserLoginValidator: AbstractValidator<LoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("{PropertyName} is reuired!");

            RuleFor(x => x.Password).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Your password cannot be empty");
        }
    }
}
