using FluentResults;
using ScoringServiceAPI.Data.Entities;

namespace ScoringServiceAPI.Services
{
    public interface IScoreService
    {
        public Task<Result<ClientEntity>> GetScoreAsync(string clientId);
        public Task<Result> AddScoreAsync(string clientId, int value);

        public Task<Result<List<ClientEntity>?>> GetAllScoresAsync();
    }
}
