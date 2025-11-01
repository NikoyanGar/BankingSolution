using ClientLoanServiceAPI.Models;
using ClientLoanServiceAPI.Models.Requests;
using ClientLoanServiceAPI.Models.Responses;

namespace ClientLoanServiceAPI.Services
{
    //Adjust naming
    public interface ILoanHistoryService
    {
        public Task AddLoanAsync(LoanHistory loanHistory);
        public Task<LoanResponse> GetLoanByClientIdAsync(LoanRequest reques);
    }
}
