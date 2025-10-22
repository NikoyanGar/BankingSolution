using FluentValidation;
using ScoringServiceAPI.Models;

namespace ScoringServiceAPI.Validators
{
    public class AddScoreValidator: AbstractValidator<AddScoreModel>
    {
        public AddScoreValidator()
        {
            RuleFor(x => x.ClientId).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Client id is required!");
            RuleFor(x => x.Score).Cascade(CascadeMode.Stop).NotEmpty().InclusiveBetween(0, 1000).WithMessage("Score must be between 0 and 1000");
        }
    }
}
