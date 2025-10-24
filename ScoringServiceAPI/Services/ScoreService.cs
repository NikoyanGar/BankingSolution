using ScoringServiceAPI.Models;
using ScoringServiceAPI.Repositories;

namespace ScoringServiceAPI.Services
{
    //2 abstractions , why ? and add interface for service
    public class ScoreService
    {
        private readonly IScoreRepository _scoreRepository;
        public ScoreService(IScoreRepository scoreRepository) => _scoreRepository = scoreRepository;

        public Task<ClientScore> GetScoreAsync(string clientId)
        {
            return _scoreRepository.GetByClientIdAsync(clientId);
        }

        public async Task AddScoreAsync(string clientId, int value)
        {
            var score = new ClientScore { ClientId = clientId, Score = value, UpdatedAt = DateTime.UtcNow };
            await _scoreRepository.AddScoreAsync(score);
        }

        public async Task<List<ClientScore>> GetAllScoresAsync()
        {
            return await _scoreRepository.GetAllAsync();
        }
    }
}
