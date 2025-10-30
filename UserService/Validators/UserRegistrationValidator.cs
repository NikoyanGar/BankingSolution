using FluentValidation;
using UserService.Models.Requests;

namespace UserService.Validators
{
    public class UserRegistrationValidator: AbstractValidator<RegisterRequest>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(4)
                .WithMessage("{PropertyName} is required.");

            RuleFor(x => x.Roles)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(roles =>
                {
                    var roleStrings = roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    foreach (var roleString in roleStrings)
                    {
                        if (!Enum.TryParse<Role>(roleString, true, out _))
                        {
                            return false;
                        }
                    }
                    return true;
                })
                .WithMessage("{PropertyName} contains invalid role(s).");

            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(4)
                .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(10)
                .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty()
                .EmailAddress().WithName("MailID").WithMessage("{PropertyName} is invalid! Please check!");

            RuleFor(x => x.Password).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Your password cannot be empty")
                .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[!@#\$%\^&\*\?_~\-\(\\\)\.]+").WithMessage("Your password must contain at least one (!? *.).");
        }

        private bool IsValidName(string name)
        {
            return name.All(char.IsLetter);
        }
    }

    public enum Role
    {
        User = 1,
        Guest = 2,
        Admin = 4,
        Manager = 8
    }
}
