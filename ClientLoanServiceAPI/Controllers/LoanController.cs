using ClientLoanServiceAPI.Models;
using ClientLoanServiceAPI.Models.Requests;
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

        [HttpPost]
        public async Task<IActionResult> CreateLoanHistory([FromBody] LoanHistory loanHistory)
        {
            var result = await _loanHistoryValidator.ValidateAsync(loanHistory);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            await _loanHistoryService.AddLoanAsync(loanHistory);
            return Created(
                nameof(GetLoanHistoryByClientId),
                new {
                    message = "Loan added successfully", loanHistory 
                }
            );
        }

        //handle not found case too
        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetLoanHistoryByClientId([FromRoute] string clientId)
        {
            var loanRequest = new LoanRequest { ClientId = clientId };
            var result = await _loanRequestValidator.ValidateAsync(loanRequest);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            var loans = await _loanHistoryService.GetLoanByClientIdAsync(loanRequest);
            if (loans == null)
            {
                return NotFound(new { message = $"No loan history found for client {clientId}" });
            }
            return Ok(loans);
        }
    }
}
