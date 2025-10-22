using BankingService.DTOs;

namespace BankingService.Clients
{
    public interface IScoringClient
    {
        public Task<ScoringDto> GetScoreAsync(string clientId);
    }
}
