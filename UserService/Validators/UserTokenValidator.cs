using FluentValidation;

namespace UserService.Validators
{
    public class UserTokenValidator: AbstractValidator<string>
    {
        public UserTokenValidator()
        {
            RuleFor(x => x).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Token is required!");
        }
    }
}
