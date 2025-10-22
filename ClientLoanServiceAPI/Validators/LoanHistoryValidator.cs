using ClientLoanServiceAPI.Models;
using FluentValidation;

namespace ClientLoanServiceAPI.Validators
{
    public class LoanHistoryValidator: AbstractValidator<LoanHistory>
    {
        public LoanHistoryValidator()
        {
            RuleFor(x => x.ClientId).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("ClientId is required.");

            RuleFor(x => x.Amount).Cascade(CascadeMode.Stop).NotEmpty()
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Approved).Cascade(CascadeMode.Stop).NotEmpty()
                .NotNull().WithMessage("Approved flag must be set (true/false).");

            RuleFor(x => x.RequestedAt).Cascade(CascadeMode.Stop).NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("RequestedAt cannot be in the future.");

            RuleFor(x => x.LoanId).Cascade(CascadeMode.Stop).NotEmpty()
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.LoanId))
                .WithMessage("LoanId cannot exceed 50 characters.");

            RuleFor(x => x.DateIssued).Cascade(CascadeMode.Stop).NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("DateIssued cannot be in the future.");

            RuleFor(x => x.IsRepaid).Cascade(CascadeMode.Stop).NotEmpty()
                .NotNull().WithMessage("IsRepaid must be specified (true/false).");
        }
    }
}
