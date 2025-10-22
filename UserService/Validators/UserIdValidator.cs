using FluentValidation;

namespace UserService.Validators
{
    public class UserIdValidator: AbstractValidator<int>
    {
        public UserIdValidator()
        {
            RuleFor(x => x).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("User id is required!");
        }
    }
}
