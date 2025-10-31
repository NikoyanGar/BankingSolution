using ClientLoanServiceAPI.Models;

namespace ClientLoanServiceAPI.Repositories
{
    //Adjust nameing
    public interface ILoanHistoryRepository
    {
        public Task AddAsync(LoanHistory loanHistory);
        public Task<List<LoanHistory>> GetByClientIdAsync(string clientId);
    }
}
