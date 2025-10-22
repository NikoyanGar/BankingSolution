using ScoringServiceAPI.Models;

namespace ScoringServiceAPI.Repositories
{
    public interface IScoreRepository
    {
        public Task<ClientScore> GetByClientIdAsync(string clientId);
        public Task AddScoreAsync(ClientScore score);
        public Task<List<ClientScore>> GetAllAsync();
    }
}
