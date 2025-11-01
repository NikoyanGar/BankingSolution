using BankingService.Models.Responses;

namespace BankingService.Clients
{
    public interface IClientLoanClient
    {
        public Task<LoanResponse> GetClientLoanHistoryAsync(string clientId);
    }
}
