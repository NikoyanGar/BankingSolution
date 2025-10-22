using Microsoft.EntityFrameworkCore;
using ScoringServiceAPI.Data;
using ScoringServiceAPI.Models;

namespace ScoringServiceAPI.Repositories
{
    public class ScoreRepository : IScoreRepository
    {
        
        private readonly ScoringDbContext _db;
        public ScoreRepository(ScoringDbContext db)
        {
            _db = db;
        }

        public async Task<ClientScore> GetByClientIdAsync(string clientId)
        {
            return await _db.Scores.FirstOrDefaultAsync(s => s.ClientId == clientId);
        }

        public async Task AddScoreAsync(ClientScore score)
        {
            var existing = await _db.Scores.FirstOrDefaultAsync(s => s.ClientId == score.ClientId);
            if (existing == null)
            {
                _db.Scores.Add(score);
            }
            else
            {
                existing.Score = score.Score;
                existing.UpdatedAt = DateTime.UtcNow;
            }
            await _db.SaveChangesAsync();
        }

        public async Task<List<ClientScore>> GetAllAsync()
        {
            return await _db.Scores.ToListAsync();
        }
    }
}
