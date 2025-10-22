using BankingService.DTOs;

namespace BankingService.Clients
{
    public interface IClientLoanClient
    {
        public Task<LoanResponseDto> GetClientLoanHistoryAsync(string clientId);
    }
}
