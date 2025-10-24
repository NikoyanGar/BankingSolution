using ClientLoanServiceAPI.Models;
using ClientLoanServiceAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ClientLoanServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //TODO: Loans Controller
    //Change response types to proper IActionResult types
    // no need for routes like addLoanHistory 
    public class LoanController : ControllerBase
    {
        private readonly ILoanHistoryService _loanHistoryService;
        private readonly IValidator<LoanRequest> _loanRequestValidator;
        private readonly IValidator<LoanHistory> _loanHistoryValidator;
        public LoanController(ILoanHistoryService loanHistoryService, IValidator<LoanRequest> loanRequestValidator, IValidator<LoanHistory> loanHistoryValidator)
        {
            _loanHistoryService = loanHistoryService;
            _loanRequestValidator = loanRequestValidator;
            _loanHistoryValidator = loanHistoryValidator;
        }

        [HttpPost("addLoanHistory")]
        public async Task<object> AddLoanHistory([FromBody] LoanHistory loanHistory)
        {
            var validationResult = await _loanHistoryValidator.ValidateAsync(loanHistory);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            await _loanHistoryService.AddLoanAsync(loanHistory);
            return Ok(
                new {
                    message = "Loan added successfully", loanHistory 
                }
            );
        }
        //handle not found case too
        [HttpPost("getLoanHistory")]
        public async Task<object> GetLoanHistory([FromBody] LoanRequest loanRequest)
        {
            var validationResult = await _loanRequestValidator.ValidateAsync(loanRequest);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var loans = await _loanHistoryService.GetLoanByClientIdAsync(loanRequest);
            return Ok(loans);
        }
    }
}
