using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ScoringServiceAPI.Models.Requests;
using ScoringServiceAPI.Models.Responses;
using ScoringServiceAPI.Services;

namespace ScoringServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoringController : ControllerBase
    {
        private readonly ScoreService _scoreService;
        private readonly IValidator<GetScoreRequest> _getScoreValidator;
        private readonly IValidator<AddScoreRequest> _addScoreValidator;
        public ScoringController(ScoreService scoreService, IValidator<GetScoreRequest> getScoreValidator, IValidator<AddScoreRequest> addScoreValidator)
        {
            _scoreService = scoreService;
            _getScoreValidator = getScoreValidator;
            _addScoreValidator = addScoreValidator;
        }

        [HttpGet("{clientId}")]
        public async Task<ActionResult<GetScoreResponse>> GetByClientId(string clientId)
        {
            var result = await _getScoreValidator.ValidateAsync(new GetScoreRequest { ClientId = clientId });
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            var score = await _scoreService.GetScoreAsync(clientId);
            if (score == null) return NotFound(new { message = "Score not found" });
            var getScoreResponse = new GetScoreResponse
            {
                ClientId = score.Value.ClientId,
                Score = score.Value.Score,
                UpdatedAt = score.Value.UpdatedAt
            };
            return Ok(getScoreResponse);
        }

        [HttpPost]
        public async Task<ActionResult<GetScoreResponse>> CreateScore([FromBody] AddScoreRequest addScoreRequest)
        {
            var result = await _addScoreValidator.ValidateAsync(addScoreRequest);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            await _scoreService.AddScoreAsync(addScoreRequest.ClientId, addScoreRequest.Score);
            var getScoreResponse = new GetScoreResponse
            {
                ClientId = addScoreRequest.ClientId,
                Score = addScoreRequest.Score,
                UpdatedAt = DateTime.Now
            };
            return Created(nameof(GetByClientId), getScoreResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _scoreService.GetAllScoresAsync();
            if (result.IsFailed)
                return BadRequest();
            return Ok(result.Value);
        }
    }
}
