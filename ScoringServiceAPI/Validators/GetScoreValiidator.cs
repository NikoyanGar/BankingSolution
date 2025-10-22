using FluentValidation;
using ScoringServiceAPI.Models;

namespace ScoringServiceAPI.Validators
{
    public class GetScoreValiidator: AbstractValidator<GetScoreModel>
    {
        public GetScoreValiidator()
        {
            RuleFor(x => x.ClientId).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Client id is required!");
        }
    }
}
