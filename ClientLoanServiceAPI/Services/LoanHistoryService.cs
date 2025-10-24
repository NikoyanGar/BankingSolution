using ClientLoanServiceAPI.Models;
using ClientLoanServiceAPI.Repositories;

namespace ClientLoanServiceAPI.Services
{
    public class LoanHistoryService: ILoanHistoryService
    {
        private readonly ILoanHistoryRepository? _loanHistoryRepository;
        private readonly decimal _requestFee;
        public LoanHistoryService(ILoanHistoryRepository loanHistoryRepository, IConfiguration config)
        {
            _loanHistoryRepository = loanHistoryRepository;
            _requestFee = config.GetValue<decimal>("ClientLoanService:RequestFee");
        }

        public async Task<LoanResponse> GetLoanByClientIdAsync(LoanRequest loanRequest)
        {
            //variable name loanJson is misleading here since it is actually a list of LoanHistory objects
            var loanJson = await _loanHistoryRepository.GetLoanByClientIdAsync(loanRequest.ClientId);

            if (loanJson == null)
            {
                return new LoanResponse();
            }

            return new LoanResponse
            {
                History = loanJson,
                FeeCharged = _requestFee
            };
        }

        public async Task AddLoanAsync(LoanHistory loanHistory)
        {
           await _loanHistoryRepository.AddLoanAsync(loanHistory);
        }

    }
}
