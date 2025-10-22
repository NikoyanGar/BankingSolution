using ClientLoanServiceAPI.Models;
using FluentValidation;

namespace ClientLoanServiceAPI.Validators
{
    public class LoanRequestValidator: AbstractValidator<LoanRequest>
    {
        public LoanRequestValidator()
        {
            RuleFor(x => x.ClientId).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Client id is required!");
        }
    }
}
