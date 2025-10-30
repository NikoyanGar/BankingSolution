using Microsoft.EntityFrameworkCore;
using ScoringServiceAPI.Data;
using ScoringServiceAPI.Models;

namespace ScoringServiceAPI.Services
{
    public class ScoreService
    {
        private readonly ScoringDbContext _db;
        public ScoreService(ScoringDbContext db)
        {
            _db = db;
        }

        public async Task<ClientScore> GetScoreAsync(string clientId)
        {
            return await _db.Scores.FirstOrDefaultAsync(s => s.ClientId == clientId);
        }

        public async Task AddScoreAsync(string clientId, int value)
        {
            var score = new ClientScore { ClientId = clientId, Score = value, UpdatedAt = DateTime.UtcNow };
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

        public async Task<List<ClientScore>> GetAllScoresAsync()
        {
            return await _db.Scores.ToListAsync();
        }
    }
}
