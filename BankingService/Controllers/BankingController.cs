using BankingService.Models;
using BankingService.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BankingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankingController : ControllerBase
    {
        private readonly IBankingService? _bankingServiceOp;
        private readonly IValidator<EvaluateLoanModel>? _evaluateLoanValidator;

        public BankingController(IBankingService bankingServiceOp, IValidator<EvaluateLoanModel> evaluateLoanValidator)
        {
            _bankingServiceOp = bankingServiceOp;
            _evaluateLoanValidator = evaluateLoanValidator;
        }

        [HttpPost("evaluate")]
        public async Task<object> EvaluateLoan([FromBody] EvaluateLoanModel evaluateLoanModel)
        {
            var validationResult = await _evaluateLoanValidator.ValidateAsync(evaluateLoanModel);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await _bankingServiceOp.EvaluateLoanAsync(evaluateLoanModel.ClientId, evaluateLoanModel.Amount);
            return Ok(result);
        }
    }
}
