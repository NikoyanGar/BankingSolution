using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ScoringServiceAPI.Models;
using ScoringServiceAPI.Services;

namespace ScoringServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // return type should be IActionResult, no need for additional routing
    public class ScoringController : ControllerBase
    {
        private readonly ScoreService _scoreService;
        private readonly IValidator<GetScoreModel> _getScoreValidator;
        private readonly IValidator<AddScoreModel> _addScoreValidator;
        public ScoringController(ScoreService scoreService, IValidator<GetScoreModel> getScoreValidator, IValidator<AddScoreModel> addScoreValidator)
        {
            _scoreService = scoreService;
            _getScoreValidator = getScoreValidator;
            _addScoreValidator = addScoreValidator;
        }

        [HttpGet("GetByClientId")]
        public async Task<IActionResult> GetScore([FromQuery] GetScoreModel scoreModel)
        {
            var result = await _getScoreValidator.ValidateAsync(scoreModel);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            var score = await _scoreService.GetScoreAsync(scoreModel.ClientId);
            if (score == null) return NotFound(new { message = "Score not found" });
            return Ok(new { clientId = score.ClientId, score = score.Score, updatedAt = score.UpdatedAt });
        }

        [HttpPost("AddScore")]
        public async Task<IActionResult> AddScore([FromBody] AddScoreModel addScoreModel)
        {
            var result = await _addScoreValidator.ValidateAsync(addScoreModel);
            if (!result.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(result.ToDictionary()));
            }

            await _scoreService.AddScoreAsync(addScoreModel.ClientId, addScoreModel.Score);
            return Ok(new { addScoreModel.ClientId, score = addScoreModel.Score });
        }

        [HttpGet("AllInfo")]
        public async Task<IActionResult> GetAllScores()
        {
            var result = await _scoreService.GetAllScoresAsync();
            return Ok(result);
        }
    }
}
