using BankingService.Models.Responses;
using FluentResults;

namespace BankingService.Clients
{
    public interface IScoringClient
    {
        public Task<Result<ScoringResponse>> GetScoreAsync(string clientId);
    }
}
