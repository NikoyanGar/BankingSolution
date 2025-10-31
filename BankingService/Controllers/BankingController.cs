using BankingService.Models.Requests;
using BankingService.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BankingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankingController : ControllerBase
    {
        private readonly IBankingService? _bankingServiceOp;
        private readonly IValidator<EvaluateLoanRequest>? _evaluateLoanValidator;

        public BankingController(IBankingService bankingServiceOp, IValidator<EvaluateLoanRequest> evaluateLoanValidator)
        {
            _bankingServiceOp = bankingServiceOp;
            _evaluateLoanValidator = evaluateLoanValidator;
        }

        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized("Missing or invalid token.");

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimConstants.UserId)?.Value;
            var name = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimConstants.Name)?.Value;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimConstants.Mail)?.Value;
            var clientId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimConstants.ClientId)?.Value;
            var roles = jwtToken.Claims.Where(c => c.Type == ClaimConstants.Roles).Select(c => c.Value).ToList();

            return Ok(new
            {
                UserId = userId,
                Name = name,
                Email = email,
                ClientId = clientId,
                Roles = roles
            });
        }

        [HttpPost("evaluate")]
        public async Task<IActionResult> EvaluateLoan([FromBody] EvaluateLoanRequest evaluateLoanModel)
        {
            var result = await _evaluateLoanValidator!.ValidateAsync(evaluateLoanModel);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            var response = await _bankingServiceOp!.EvaluateLoanAsync(evaluateLoanModel.ClientId, evaluateLoanModel.Amount);
            if (response.IsFailed)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = response.Errors });

            return Ok(response.Value);
        }
    }
}
