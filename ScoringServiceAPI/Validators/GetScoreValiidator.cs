using FluentValidation;
using ScoringServiceAPI.Models.Requests;

namespace ScoringServiceAPI.Validators
{
    public class GetScoreValiidator: AbstractValidator<GetScoreRequest>
    {
        public GetScoreValiidator()
        {
            RuleFor(x => x.ClientId).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Client id is required!");
        }
    }
}
