using ClientLoanServiceAPI.Models;

namespace ClientLoanServiceAPI.Services
{
    public interface ILoanHistoryService
    {
        public Task AddLoanAsync(LoanHistory loanHistory);
        public Task<LoanResponse> GetLoanByClientIdAsync(LoanRequest reques);
    }
}
