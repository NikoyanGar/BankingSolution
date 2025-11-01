using FluentValidation;
using UserService.Models.Requests;

namespace UserService.Validators
{
    public class UserLoginValidator: AbstractValidator<LoginRequest>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("{PropertyName} is reuired!");

            RuleFor(x => x.Password).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Your password cannot be empty");
        }
    }
}
