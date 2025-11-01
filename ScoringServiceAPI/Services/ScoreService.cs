using FluentResults;
using Microsoft.EntityFrameworkCore;
using ScoringServiceAPI.Data;
using ScoringServiceAPI.Data.Entities;

namespace ScoringServiceAPI.Services
{
    public class ScoreService: IScoreService
    {
        private readonly ScoringDbContext _db;
        public ScoreService(ScoringDbContext db)
        {
            _db = db;
        }

        public async Task<Result<ClientEntity>> GetScoreAsync(string clientId)
        {
            try
            {
                var score = await _db.Scores.FirstOrDefaultAsync(s => s.ClientId == clientId);
                return Result.Ok<ClientEntity>(score);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result> AddScoreAsync(string clientId, int value)
        {
            try
            {
                var score = new ClientEntity { ClientId = clientId, Score = value, UpdatedAt = DateTime.UtcNow };
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
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result<List<ClientEntity>?>> GetAllScoresAsync()
        {
            try
            {
                var scores = await _db.Scores.ToListAsync();
                if (scores is null)
                    return Result.Ok<List<ClientEntity>?>(new List<ClientEntity>());
                return Result.Ok(scores);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

    }
}
