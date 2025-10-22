using ClientLoanServiceAPI.Models;

namespace ClientLoanServiceAPI.Repositories
{
    public interface ILoanHistoryRepository
    {
        public Task AddLoanAsync(LoanHistory loanHistory);
        public Task<List<LoanHistory>> GetLoanByClientIdAsync(string clientId);
    }
}
