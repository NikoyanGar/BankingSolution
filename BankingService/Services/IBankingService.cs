using BankingService.Clients;
using BankingService.Models;

namespace BankingService.Services
{
    public class IBankingService
    {
        private readonly IClientLoanClient? _clientLoanClient;
        private readonly IScoringClient? _scoringClient;

        public IBankingService(IClientLoanClient clientLoanClient, IScoringClient scoringClient)
        {
            _clientLoanClient = clientLoanClient;
            _scoringClient = scoringClient;
        }

        public async Task<LoanDecision> EvaluateLoanAsync(string clientId, decimal amount)
        {
            var score = await _scoringClient.GetScoreAsync(clientId);

            if (score == null)
            {
                return new LoanDecision
                {
                    ClientId = clientId,
                    RequestedAmount = amount,
                    Decision = "Declined",
                    Reason = "Score not found"
                };
            }

            var loanHistory = await _clientLoanClient.GetClientLoanHistoryAsync(clientId);
            if (loanHistory.History.Count(l => !l.IsRepaid) > 1)
            {
                return new LoanDecision
                {
                    ClientId = clientId,
                    RequestedAmount = amount,
                    Decision = "Declined",
                    Reason = "Too many unpaid loans"
                };
            }

            if (score.Score < 600)
            {
                return new LoanDecision
                {
                    ClientId = clientId,
                    RequestedAmount = amount,
                    Decision = "Declined",
                    Reason = "Low credit score"
                };
            }

            return new LoanDecision
            {
                ClientId = clientId,
                RequestedAmount = amount,
                Decision = "Approved",
                Reason = "Good credit history and score"
            };
        }
    }
}
