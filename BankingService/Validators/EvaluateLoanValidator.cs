using BankingService.Models;
using FluentValidation;

namespace BankingService.Validators
{
    public class EvaluateLoanValidator: AbstractValidator<EvaluateLoanModel>
    {
        public EvaluateLoanValidator()
        {
            RuleFor(x => x.ClientId).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("ClientId is required.");

            RuleFor(x => x.Amount).Cascade(CascadeMode.Stop).NotEmpty()
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }
    }
}
