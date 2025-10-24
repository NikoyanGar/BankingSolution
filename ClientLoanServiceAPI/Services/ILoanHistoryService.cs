using ClientLoanServiceAPI.Models;

namespace ClientLoanServiceAPI.Services
{
    //Adjust naming
    public interface ILoanHistoryService
    {
        public Task AddLoanAsync(LoanHistory loanHistory);
        public Task<LoanResponse> GetLoanByClientIdAsync(LoanRequest reques);
    }
}
